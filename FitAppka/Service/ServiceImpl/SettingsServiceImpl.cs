using FitAppka.Model;
using FitAppka.Repository;
using System;

namespace FitAppka.Service.ServiceImpl
{
    public class SettingsServiceImpl : ISettingsService {

        private readonly IDietaryTargetsService _dietaryTargetsService;
        private readonly IDayRepository _dayRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IMeasurementsService _measurementsService;

        public SettingsServiceImpl(IDayRepository dayRepository, IClientRepository clientRepository, 
            IDietaryTargetsService dietaryTargetsService, IMeasurementsService measurementsService)
        {
            _measurementsService = measurementsService;
            _dietaryTargetsService = dietaryTargetsService;
            _clientRepository = clientRepository;
            _dayRepository = dayRepository;
        }

        public void ChangeSettings(SettingsModel m, int isFirstLaunch)
        {
            Client client = _clientRepository.GetLoggedInClient();
            SetClientGoals(m, client);
            SetDateOfJoiningIfFirstLaunch(isFirstLaunch, client);
            MapDayMealsFromClientToDaysFromToday(m, client);
            _dietaryTargetsService.UpdateTargetsInDaysFromToday(client);
            SetClientData(m, client);
            SetClientWeightMeasurement(m);
        }

        private void SetDateOfJoiningIfFirstLaunch(int isFirstLaunch, Client client)
        {
            if (isFirstLaunch == 1) { 
                client.DateOfJoining = DateTime.Now.Date; 
            }
        }

        private void SetClientGoals(SettingsModel m, Client client) {
            if ((bool) client.AutoDietaryGoals) 
            {
                double pace = 0.4;
                try { pace = double.Parse(m.PaceOfChange.Replace('.', ',').Replace(" ", "")); } catch { }

                int caloricDemand = _dietaryTargetsService.CountCaloricDemand(m.Sex, m.Date_of_birth, m.Growth, m.Weight, ActivityLevel(m.LevelOfActivity));
                int calorieTarget = _dietaryTargetsService.CountCalorieTarget(caloricDemand, m.WeightChange_Goal, pace);
                int proteinTarget = _dietaryTargetsService.CountProteinTarget(m.Weight, calorieTarget, m.LevelOfActivity);
                int fatTarget = _dietaryTargetsService.CountFatTarget(calorieTarget);
                int carbsTarget = _dietaryTargetsService.CountCarbsTarget(calorieTarget, proteinTarget, fatTarget);

                client.CaloricDemand = caloricDemand;
                client.CalorieGoal = calorieTarget;
                client.ProteinTarget = proteinTarget;
                client.FatTarget = fatTarget;
                client.CarbsTarget = carbsTarget;
                client.PaceOfChanges = pace;
            }
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

        private void MapDayMealsFromClientToDaysFromToday(SettingsModel m, Client client)
        {
            foreach (var dayID in _dietaryTargetsService.GetListOfDaysIDFromToday(client))
            {
                foreach (var day in _dayRepository.GetAllDays())
                {
                    if (day.DayId == dayID)
                    {
                        day.Breakfast = m.Breakfast.GetValueOrDefault(false);
                        day.Lunch = m.Lunch.GetValueOrDefault(false);
                        day.Dinner = m.Dinner.GetValueOrDefault(false);
                        day.Dessert = m.Dessert.GetValueOrDefault(false);
                        day.Snack = m.Snack.GetValueOrDefault(false);
                        day.Supper = m.Supper.GetValueOrDefault(false);
                    }
                }
            }
        }

        private void SetClientWeightMeasurement(SettingsModel m)
        {
            _measurementsService.AddOrUpdateMeasurements((short)m.Weight, 0);
        }

        private void SetClientData(SettingsModel m, Client client)
        {
            client.DateOfBirth = m.Date_of_birth;
            client.Growth = m.Growth;
            client.Sex = m.Sex.GetValueOrDefault(false);
            client.Breakfast = m.Breakfast.GetValueOrDefault(false);
            client.Lunch = m.Lunch.GetValueOrDefault(false);
            client.Dinner = m.Dinner.GetValueOrDefault(false);
            client.Dessert = m.Dessert.GetValueOrDefault(false);
            client.Snack = m.Snack.GetValueOrDefault(false);
            client.Supper = m.Supper.GetValueOrDefault(false);
            client.WeightChangeGoal = m.WeightChange_Goal;
            client.ActivityLevel = m.LevelOfActivity;
        }

    }
}