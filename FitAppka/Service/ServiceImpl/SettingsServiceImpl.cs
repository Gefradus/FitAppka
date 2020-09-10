using FitAppka.Models;
using FitAppka.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FitAppka.Service.ServiceImpl
{
    public class SettingsServiceImpl : ISettingsService {

        private readonly IDayRepository _dayRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IWeightMeasurementRepository _weightMeasurementRepository;
        private readonly FitAppContext _context;

        public SettingsServiceImpl(IDayRepository dayRepository, IClientRepository clientRepository, 
            IWeightMeasurementRepository weightMeasurementRepository, FitAppContext context)
        {
            _context = context;
            _weightMeasurementRepository = weightMeasurementRepository;
            _clientRepository = clientRepository;
            _dayRepository = dayRepository;
        }

        public async Task ChangeSettings(SettingsModel m, int isFirstLaunch)
        {
            Client client = _clientRepository.GetLoggedInClient();
            SetClientGoals(m, client);
            SetDateOfJoiningIfFirstLaunch(isFirstLaunch, client);
            SetDataForDaysFromToday(m, client);
            SetClientWeightMeasurement(m, SetClientData(m, client));
            await _context.SaveChangesAsync();
        }

        private void SetDateOfJoiningIfFirstLaunch(int isFirstLaunch, Client client)
        {
            if (isFirstLaunch == 1) { 
                client.DateOfJoining = DateTime.Now.Date; 
            }
        }

        private void SetClientGoals(SettingsModel m, Client client) {
            double pace = 0.4;
            try { pace = double.Parse(m.PaceOfChange.Replace('.', ',').Replace(" ", "")); } catch { }

            int caloricDemand = CountCaloricDemand(m.Sex, m.Date_of_birth, m.Growth, m.Weight, ActivityLevel(m.LevelOfActivity));
            int calorieTarget = CountCalorieTarget(caloricDemand, m.WeightChange_Goal, pace);
            int proteinTarget = CountProteinTarget(m.Weight, calorieTarget, m.LevelOfActivity);
            int fatTarget = CountFatTarget(calorieTarget);
            int carbsTarget = CountCarbsTarget(calorieTarget, proteinTarget, fatTarget);

            client.CalorieGoal = calorieTarget;
            client.CaloricDemand = caloricDemand;
            client.ProteinTarget = proteinTarget;
            client.FatTarget = fatTarget;
            client.CarbsTarget = carbsTarget;
            client.PaceOfChanges = pace;
        }

        private double ActivityLevel(short? activity)
        {
            if (activity == 1) { return 1.2; }
            if (activity == 2) { return 1.3; }
            if (activity == 3) { return 1.5; }
            if (activity == 4) { return 1.7; }
            if (activity == 5) { return 1.9; }
            return 1.5;
        }

        private int CountCalorieTarget(int demand, short? changeGoal, double pace) {
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
                    return (int)demand;
                }
            }

        private int CountCaloricDemand(bool? sex, DateTime? birthDate, int? growth, double? weight, double activity)
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

        private int CountProteinTarget(double? weight, int kcalTarget, short? activity)
        {
            int proteins;
            if (activity < 3)
            {
                proteins = (int)weight;
                if (proteins < kcalTarget)
                {
                    return proteins;
                }
                else
                {
                    return (int)(0.3 * kcalTarget);
                }
            }
            else if (activity == 3 || activity == 4)
            {
                proteins = (int)(1.5 * weight);
                if (proteins < kcalTarget)
                {
                    return proteins;
                }
                else
                {
                    return (int)(0.5 * kcalTarget);
                }
            }
            else
            {
                proteins = (int)(2 * weight);
                if (proteins < kcalTarget)
                {
                    return proteins;
                }
                else
                {
                    return (int)(0.65 * kcalTarget);
                }
            }
        }

        private int CountFatTarget(int kcalTarget)
        {
            return kcalTarget / 36;    //25% of demand from fats (1g of fat has 9kcal)
        }

        private int CountCarbsTarget(int kcalTarget, int proteinsTarget, int fatsTarget)
        {
            return (kcalTarget - ((proteinsTarget * 4) + (fatsTarget * 9))) / 4;
        }


        private int CountAge(DateTime birthDate)
        {
            int age = DateTime.Today.Year - birthDate.Year;
            if (birthDate.Date > DateTime.Today.AddYears(-age)) age--;
            return age;
        }

        private void SetDataForDaysFromToday(SettingsModel m, Client client)
        {
            foreach (var dayID in GetListOfDaysIDFromToday(client))
            {
                foreach (var day in _dayRepository.GetAllDays())
                {
                    if (day.DayId == dayID)
                    {
                        day.CalorieTarget = client.CalorieGoal;
                        day.ProteinTarget = client.ProteinTarget;
                        day.FatTarget = client.FatTarget;
                        day.CarbsTarget = client.CarbsTarget;
                        day.Breakfast = m.Breakfast;
                        day.Lunch = m.Lunch;
                        day.Dinner = m.Dinner;
                        day.Dessert = m.Dessert;
                        day.Snack = m.Snack;
                        day.Supper = m.Supper;
                        if (m.Breakfast == null) { day.Breakfast = false; }
                        if (m.Lunch == null) { day.Lunch = false; }
                        if (m.Dinner == null) { day.Dinner = false; }
                        if (m.Dessert == null) { day.Dessert = false; }
                        if (m.Snack == null) { day.Snack = false; }
                        if (m.Supper == null) { day.Supper = false; }
                    }
                }
            }
        }

        private void SetClientWeightMeasurement(SettingsModel m, Client client)
        {
            client.WeightMeasurement.Add(new WeightMeasurement()
            {
                DateOfMeasurement = DateTime.Now,
                Weight = (double)m.Weight,

            });

            _clientRepository.Update(client);
        }

        private List<int> GetListOfDaysIDFromToday(Client client)
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

        private Client SetClientData(SettingsModel m, Client client)
        {
            client.DateOfBirth = m.Date_of_birth;
            client.Sex = m.Sex;
            client.Growth = m.Growth;
            client.Breakfast = m.Breakfast;
            client.Lunch = m.Lunch;
            client.Dinner = m.Dinner;
            client.Dessert = m.Dessert;
            client.Snack = m.Snack;
            client.Supper = m.Supper;
            client.WeightChangeGoal = m.WeightChange_Goal;
            client.ActivityLevel = m.LevelOfActivity;
            if (m.Breakfast == null) { client.Breakfast = false; }
            if (m.Lunch == null) { client.Lunch = false; }
            if (m.Dinner == null) { client.Dinner = false; }
            if (m.Dessert == null) { client.Dessert = false; }
            if (m.Snack == null) { client.Snack = false; }
            if (m.Supper == null) { client.Supper = false; }

            return client;
        }


        public double SetLastWeightMeasurement()
        {
            var measurementList = _weightMeasurementRepository.GetClientsWeightMeasurements(_clientRepository.GetLoggedInClient());

            double lastWeightMeasurement = 0;
            foreach (var item in measurementList)
            {
                if (ProtectionAgainstTheMinimum(measurementList) == item.DateOfMeasurement)
                {
                    lastWeightMeasurement = item.Weight;
                }
            }

            return lastWeightMeasurement;
        }

        private DateTime? ProtectionAgainstTheMinimum(IEnumerable<WeightMeasurement> measurementList)
        {
            DateTime? dateOfMeasurement = DateTime.MinValue;

            foreach (WeightMeasurement measurement in measurementList)
            {
                if (dateOfMeasurement < measurement.DateOfMeasurement)
                {
                    dateOfMeasurement = measurement.DateOfMeasurement;
                }
            }

            return dateOfMeasurement;
        }

  
    }
}