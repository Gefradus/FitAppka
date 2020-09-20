using FitAppka.Models;
using FitAppka.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FitAppka.Service.ServiceImpl
{
    public class GoalsServiceImpl : IGoalsService
    {
        private readonly IClientRepository _clientRepository;
        private readonly ICardioTrainingRepository _cardioRepository;
        private readonly IDayRepository _dayRepository;
        private readonly IGoalsRepository _goalsRepository;
        private readonly IWeightMeasurementRepository _weightMeasurementRepository;

        public GoalsServiceImpl(IGoalsRepository goalsRepository, IWeightMeasurementRepository weightMeasurementRepository,
            IDayRepository dayRepository, ICardioTrainingRepository cardioRepository, IClientRepository clientRepository)
        {
            _weightMeasurementRepository = weightMeasurementRepository;
            _goalsRepository = goalsRepository;
            _cardioRepository = cardioRepository;
            _dayRepository = dayRepository;
            _clientRepository = clientRepository;
        }

        public int CountCalorieTarget(int demand, short? changeGoal, double pace) {
            if (changeGoal == 1) {
                int kcalTarget = (int)(demand - (pace * 1100));
                if (kcalTarget <= 1000) {
                    return 1000;
                } else {
                    return kcalTarget;
                }
            }
            if (changeGoal == 3) {
                int kcalTarget = (int)(demand + (pace * 1100));
                if (kcalTarget <= 1000) {
                    return 1000;
                } else {
                    return kcalTarget;
                }
            }
            if (demand < 1000) {
                return 1000;
            } else {
                return demand;
            }
        }

        public int CountCaloricDemand(bool? sex, DateTime? birthDate, int? growth, double? weight, double activity)
        {
            int BMR;
            int age = CountAge((DateTime)birthDate);
            
            if ((bool)sex) { //Harris-Benedict's method   
                BMR = (int)(66 + (13.7 * weight) + (5 * growth) - (6.76 * age));
            } else {
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

        public double ActivityLevel(short? activity)
        {
            if (activity == 1) { return 1.2; }
            if (activity == 2) { return 1.3; }
            if (activity == 3) { return 1.5; }
            if (activity == 4) { return 1.7; }
            if (activity == 5) { return 1.9; }
            return 1.5;
        }

        public int CountAge(DateTime birthDate)
        {
            int age = DateTime.Today.Year - birthDate.Year;
            if (birthDate.Date > DateTime.Today.AddYears(-age)) age--;
            return age;
        }


        public List<int> GetListOfDaysIDFromToday(int clientId)
        {
            List<int> listOfIDDaysFromToday = new List<int>();
            foreach (var item in _dayRepository.GetClientDays(clientId))
            {
                if (item.Date >= DateTime.Now.Date)
                {
                    listOfIDDaysFromToday.Add(item.DayId);
                }
            }

            return listOfIDDaysFromToday;
        }

        public void UpdateGoalsInDaysFromToday()
        {
            int clientId = _clientRepository.GetLoggedInClientId();
            foreach (var dayID in GetListOfDaysIDFromToday(clientId))
            {
                foreach (var day in _dayRepository.GetClientDays(clientId))
                {
                    if (day.DayId == dayID)
                    {
                        _goalsRepository.Update(MapGoalsFromClientToDay(day, clientId));
                    }
                }
            }
        }

        private Goals MapGoalsFromClientToDay(Day day, int clientId)
        {
            Goals dayGoals = CreateIfNotExistsAndGetDayGoals(day.DayId);
            Goals clientGoals = _goalsRepository.GetClientGoals(clientId);

            if ((bool)_clientRepository.GetClientById(clientId).IncludeCaloriesBurned)
            {
                int kcalBurned = CaloriesBurnedInDay(day.DayId);
                dayGoals.Calories = clientGoals.Calories + kcalBurned;
                dayGoals.Proteins = CountTargetWithBurnedKcal(kcalBurned, clientGoals.Proteins, 4, clientGoals.Calories);
                dayGoals.Fats = CountTargetWithBurnedKcal(kcalBurned, clientGoals.Fats, 9, clientGoals.Calories);
                dayGoals.Carbohydrates = CountTargetWithBurnedKcal(kcalBurned, clientGoals.Carbohydrates, 4, clientGoals.Calories);
            }
            else
            {
                dayGoals.Calories = clientGoals.Calories;
                dayGoals.Proteins = clientGoals.Proteins;
                dayGoals.Fats = clientGoals.Fats;
                dayGoals.Carbohydrates = clientGoals.Carbohydrates;
            }

            dayGoals.KcalBurned = clientGoals.KcalBurned;
            dayGoals.TrainingTime = clientGoals.TrainingTime;
            return dayGoals;
        }

        private int CountTargetWithBurnedKcal(int kcalBurned, double target, int multiplier, double kcalGoal)
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

        private Goals CreateIfNotExistsAndGetDayGoals(int dayID)
        {
            Goals dayGoals = _goalsRepository.GetDayGoals(dayID);
            if (dayGoals == null) {
                return _goalsRepository.Add(new Goals() { DayId = dayID });
            }
            else {
                return dayGoals;
            }
        }

        public void UpdateGoals(CreateGoalsModel m)
        {
            SetClientPreferences(m, SetClientGoalsIfAutoDietaryGoals(m.AutoDietaryGoals));
            UpdateGoalsInDaysFromToday();
        }

        private void SetClientPreferences(CreateGoalsModel m, Goals clientGoals)
        {
            Client client = _clientRepository.GetLoggedInClient();
            client.AutoDietaryGoals = m.AutoDietaryGoals;
            client.IncludeCaloriesBurned = m.IncludeCaloriesBurned;
            _clientRepository.Update(client);
            clientGoals.TrainingTime = m.TrainingTimeGoal;
            clientGoals.KcalBurned = m.KcalBurnedGoal;

            if (!m.AutoDietaryGoals) {
                UpdateClientGoals(clientGoals, (int)m.CalorieGoal, (int)m.ProteinTarget, (int)m.FatTarget, (int)m.CarbsTarget);
            }
        }

        public Goals AddOrUpdateClientGoals(Client client, int calorieTarget, int proteinTarget, int fatTarget, int carbsTarget)
        {
            Goals clientGoals = _goalsRepository.GetClientGoals(client.ClientId);
            if (clientGoals != null) {
                return UpdateClientGoals(clientGoals, calorieTarget, proteinTarget, fatTarget, carbsTarget);
            } 
            else {
                return AddClientGoals(client, calorieTarget, proteinTarget, fatTarget, carbsTarget);
            }
        }

        private Goals UpdateClientGoals(Goals clientGoals, int calorieTarget, int proteinTarget, int fatTarget, int carbsTarget)
        {
            clientGoals.Calories = calorieTarget;
            clientGoals.Carbohydrates = carbsTarget;
            clientGoals.Proteins = proteinTarget;
            clientGoals.Fats = fatTarget;
            return _goalsRepository.Update(clientGoals);
        }


        private Goals AddClientGoals(Client client, int calorieTarget, int proteinTarget, int fatTarget, int carbsTarget)
        {
            return _goalsRepository.Add(new Goals()
            {
                ClientId = client.ClientId,
                Calories = calorieTarget,
                Carbohydrates = carbsTarget,
                Fats = fatTarget,
                Proteins = proteinTarget
            });
        }


        public Goals SetClientGoalsIfAutoDietaryGoals(bool autoDietaryGoals)
        {
            Client c = _clientRepository.GetLoggedInClient();
            short weight = _weightMeasurementRepository.GetLastLoggedInClientWeightMeasurement();
            if (autoDietaryGoals)
            {
                int caloricDemand = CountCaloricDemand(c.Sex, c.DateOfBirth, c.Growth, weight, ActivityLevel(c.ActivityLevel));
                int calorieTarget = CountCalorieTarget(caloricDemand, c.WeightChangeGoal, (double)c.PaceOfChanges);
                int proteinTarget = CountProteinTarget(weight, calorieTarget, c.ActivityLevel);
                int fatTarget = CountFatTarget(calorieTarget);
                int carbsTarget = CountCarbsTarget(calorieTarget, proteinTarget, fatTarget);

                c.CaloricDemand = caloricDemand;
                _clientRepository.Update(c);
                return AddOrUpdateClientGoals(c, calorieTarget, proteinTarget, fatTarget, carbsTarget);
            }

            return _goalsRepository.GetClientGoals(c.ClientId);
        }

    }
}
