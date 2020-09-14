using FitAppka.Models;
using FitAppka.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FitAppka.Service.ServiceImpl
{
    public class DietaryTargetsServiceImpl : IDietaryTargetsService
    {
        private readonly IWeightMeasurementRepository _weightMeasurementRepository;
        private readonly IClientRepository _clientRepository;
        private readonly ICardioTrainingRepository _cardioRepository;
        private readonly IDayRepository _dayRepository;

        public DietaryTargetsServiceImpl(IWeightMeasurementRepository weightMeasurementRepository, 
            IClientRepository clientRepository, IDayRepository dayRepository, ICardioTrainingRepository cardioRepository)
        {
            _cardioRepository = cardioRepository;
            _dayRepository = dayRepository;
            _weightMeasurementRepository = weightMeasurementRepository;
            _clientRepository = clientRepository;
        }

        public int CountCalorieTarget(int demand, short? changeGoal, double pace)
        {
            if (changeGoal == 1) {
                int kcalTarget = (int)(demand - (pace * 1100));

                if (kcalTarget <= 1000) {
                    return 1000;
                }
                else {
                    return kcalTarget;
                }
            }
            if (changeGoal == 3) {
                int kcalTarget = (int)(demand + (pace * 1100));

                if (kcalTarget <= 1000) {
                    return 1000;
                }
                else {
                    return kcalTarget;
                }
            }
            if (demand < 1000) {
                return 1000;
            }
            else {
                return demand;
            }
        }

        public int CountCaloricDemand(bool? sex, DateTime? birthDate, int? growth, double? weight, double activity)
        {
            int BMR;
            int age = CountAge((DateTime)birthDate);

            //Harris-Benedict's method   
            if ((bool)sex) {
                BMR = (int)(66 + (13.7 * weight) + (5 * growth) - (6.76 * age));
            }
            else {
                BMR = (int)(655 + (9.6 * weight) + (1.85 * growth) - (4.7 * age));
            }

            return (int)(BMR * activity);
        }

        public int CountProteinTarget(double? weight, int kcalTarget, short? activity)
        {
            int proteins;
            if (activity < 3) {
                proteins = (int)weight;
                if (proteins < kcalTarget) {
                    return proteins;
                }
                else {
                    return (int)(0.3 * kcalTarget);
                }
            }
            else if (activity == 3 || activity == 4) {
                proteins = (int)(1.5 * weight);
                if (proteins < kcalTarget) {
                    return proteins;
                }
                else {
                    return (int)(0.5 * kcalTarget);
                }
            }
            else {
                proteins = (int)(2 * weight);
                if (proteins < kcalTarget) {
                    return proteins;
                }
                else {
                    return (int)(0.65 * kcalTarget);
                }
            }
        }

        public int CountFatTarget(int kcalTarget)
        {
            return kcalTarget / 36;    //25% of demand from fats (1g of fat has 9kcal)
        }

        public int CountCarbsTarget(int kcalTarget, int proteinsTarget, int fatsTarget)
        {
            return (kcalTarget - ((proteinsTarget * 4) + (fatsTarget * 9))) / 4;
        }

        public int CountAge(DateTime birthDate)
        {
            int age = DateTime.Today.Year - birthDate.Year;
            if (birthDate.Date > DateTime.Today.AddYears(-age)) age--;
            return age;
        }

        public short GetLastWeightMeasurement()
        {
            var measurementList = _weightMeasurementRepository.GetClientsWeightMeasurements(_clientRepository.GetLoggedInClient());

            short lastWeightMeasurement = 0;
            foreach (var item in measurementList)
            {
                if (GetLastMeasurementDate(measurementList) == item.DateOfMeasurement)
                {
                    lastWeightMeasurement = item.Weight;
                    break;
                }
            }

            return lastWeightMeasurement;
        }

        private DateTime? GetLastMeasurementDate(IEnumerable<WeightMeasurement> measurementList)
        {
            DateTime? dateOfMeasurement = DateTime.MinValue;

            foreach (var measurement in measurementList)
            {
                if (dateOfMeasurement < measurement.DateOfMeasurement)
                {
                    dateOfMeasurement = measurement.DateOfMeasurement;
                }
            }

            return dateOfMeasurement;
        }

        public List<int> GetListOfDaysIDFromToday(Client client)
        {
            List<int> listOfIDDaysFromToday = new List<int>();
            foreach (var item in _dayRepository.GetClientDays(client.ClientId))
            {
                if (item.Date >= DateTime.Now.Date)
                {
                    listOfIDDaysFromToday.Add(item.DayId);
                }
            }

            return listOfIDDaysFromToday;
        }


        public void UpdateTargetsInDaysFromToday(Client client)
        {
            foreach (var dayID in GetListOfDaysIDFromToday(client))
            {
                foreach (var day in _dayRepository.GetClientDays(client.ClientId))
                {
                    if (day.DayId == dayID)
                    {
                        _dayRepository.Update(MapTargetsFromClientToDay(day, client));
                    }
                }
            }
        }

        private Day MapTargetsFromClientToDay(Day day, Client client)
        {
            if ((bool)client.IncludeCaloriesBurned)
            {
                int kcalBurned = CaloriesBurnedInDay(day.DayId);
                day.CalorieGoal = client.CalorieGoal + kcalBurned;
                day.ProteinTarget = CountTargetWithBurnedKcal(kcalBurned, client.ProteinTarget, 4, client.CalorieGoal);
                day.FatTarget = CountTargetWithBurnedKcal(kcalBurned, client.FatTarget, 9, client.CalorieGoal);
                day.CarbsTarget = CountTargetWithBurnedKcal(kcalBurned, client.CarbsTarget, 4, client.CalorieGoal);
            }
            else
            {
                day.CalorieGoal = client.CalorieGoal;
                day.ProteinTarget = client.ProteinTarget;
                day.FatTarget = client.FatTarget;
                day.CarbsTarget = client.CarbsTarget;
            }

            day.KcalBurnedGoal = client.KcalBurnedGoal;
            day.TrainingTimeGoal = client.TrainingTimeGoal;
            return day;
        }


        private int CountTargetWithBurnedKcal(int kcalBurned, int? target, int multiplier, int? kcalGoal)
        {
            double proportion = ((double)(target * multiplier)) / (double)kcalGoal;
            return (int)(kcalBurned * proportion + target);
        }

        public int CaloriesBurnedInDay(int dayID)
        {
            int? kcal = 0;
            foreach (CardioTraining cardio in _cardioRepository.GetAllCardioTrainings().Where(t => t.DayId.Equals(dayID))) {
                kcal += cardio.CaloriesBurned;
            }

            return (int)kcal;
        }
    }
}
