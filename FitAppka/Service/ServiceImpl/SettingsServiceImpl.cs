using FitAppka.Models;
using FitAppka.Repository;

namespace FitAppka.Service.ServiceImpl
{
    public class SettingsServiceImpl : ISettingsService {
        private readonly IDayRepository _dayRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IMeasurementsService _measurementsService;
        private readonly IGoalsService _goalsService;

        public SettingsServiceImpl(IDayRepository dayRepository, IClientRepository clientRepository,
            IGoalsService goalsService, IMeasurementsService measurementsService)
        {
            _goalsService = goalsService;
            _measurementsService = measurementsService;
            _clientRepository = clientRepository;
            _dayRepository = dayRepository;
        }

        public void ChangeSettings(SettingsDTO m, int isFirstLaunch, int clientId)
        {
            Client client = _clientRepository.GetClientById(clientId);
            SetClientGoals(m, client);
            MapDayMealsFromClientToDaysFromToday(m, clientId);
            _goalsService.UpdateGoalsInLoggedInClientDaysFromToday();
            SetClientData(m, client);
            SetClientWeightMeasurement(m, isFirstLaunch);
        }

        private void SetClientGoals(SettingsDTO m, Client client) {
            if ((bool) client.AutoDietaryGoals) 
            {
                double pace = 0.4;
                try { pace = double.Parse(m.PaceOfChanges.Replace('.', ',').Replace(" ", "")); } catch { }

                int caloricDemand = _goalsService.CountCaloricDemand(m.Sex, m.DateOfBirth, m.Growth, m.Weight, _goalsService.ActivityLevel(m.ActivityLevel));
                int calorieTarget = _goalsService.CountCalorieTarget(caloricDemand, m.WeightChangeGoal, pace);
                int proteinTarget = _goalsService.CountProteinTarget(m.Weight, calorieTarget, m.ActivityLevel);
                int fatTarget = _goalsService.CountFatTarget(calorieTarget);
                int carbsTarget = _goalsService.CountCarbsTarget(calorieTarget, proteinTarget, fatTarget);

                _goalsService.AddOrUpdateClientGoals(client, calorieTarget, proteinTarget, fatTarget, carbsTarget);
                client.CaloricDemand = caloricDemand;
                client.PaceOfChanges = pace;
            }
        }

        private void MapDayMealsFromClientToDaysFromToday(SettingsDTO m, int clientId)
        {
            foreach (var dayID in _goalsService.GetListOfDaysIDFromToday(clientId))
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

        private void SetClientWeightMeasurement(SettingsDTO m, int isItFirstLaunch)
        {
            if (isItFirstLaunch == 1) {
                _measurementsService.AddMeasurements((short)m.Weight, null);
            }
        }

        private void SetClientData(SettingsDTO m, Client client)
        {
            client.DateOfBirth = m.DateOfBirth;
            client.Growth = m.Growth;
            client.Sex = m.Sex.GetValueOrDefault(false);
            client.Breakfast = m.Breakfast.GetValueOrDefault(false);
            client.Lunch = m.Lunch.GetValueOrDefault(false);
            client.Dinner = m.Dinner.GetValueOrDefault(false);
            client.Dessert = m.Dessert.GetValueOrDefault(false);
            client.Snack = m.Snack.GetValueOrDefault(false);
            client.Supper = m.Supper.GetValueOrDefault(false);
            client.WeightChangeGoal = m.WeightChangeGoal;
            client.ActivityLevel = m.ActivityLevel;
            _clientRepository.Update(client);
        }

    }
}