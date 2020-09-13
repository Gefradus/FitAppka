using FitAppka.Models;
using FitAppka.Repository;
using System;
using System.Threading.Tasks;

namespace FitAppka.Service.ServiceImpl
{
    public class SettingsServiceImpl : ISettingsService {

        private readonly IDietaryTargetsService _dietaryTargetsService;
        private readonly IDayRepository _dayRepository;
        private readonly IClientRepository _clientRepository;
        private readonly FitAppContext _context;

        public SettingsServiceImpl(IDayRepository dayRepository, IClientRepository clientRepository, 
            FitAppContext context, IDietaryTargetsService dietaryTargetsService)
        {
            _dietaryTargetsService = dietaryTargetsService;
            _context = context;
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

        private void SetDataForDaysFromToday(SettingsModel m, Client client)
        {
            foreach (var dayID in _dietaryTargetsService.GetListOfDaysIDFromToday(client))
            {
                foreach (var day in _dayRepository.GetAllDays())
                {
                    if (day.DayId == dayID)
                    {
                        day.CalorieGoal = client.CalorieGoal;
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
                Weight = (short)m.Weight,
            });

            _clientRepository.Update(client);
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

    }
}