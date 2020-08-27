using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FitAppka.Models;
using FitAppka.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitAppka.Controllers
{
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    [Authorize]
    public class SettingsController : Controller
    {
        private readonly FitAppContext _context;
        private readonly IClientRepository _clientRepository;
        private readonly IDayRepository _dayRepository;
        private readonly SettingsServices service;

        public SettingsController(FitAppContext context, IDayRepository dayRepository, IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
            service = new SettingsServices();
            _context = context;
            _dayRepository = dayRepository;
        }

        [HttpGet]
        public IActionResult Settings()
        {
            FirstAppLaunch();
            ViewData["clientID"] = _clientRepository.GetClientByLogin(User.Identity.Name).ClientId;
            return View();
        }

        private void FirstAppLaunch()//(int clientID)
        {
            try     //jeśli try się powiedzie znaczy to że użytkownik już podał dane
            {
                Client client = _clientRepository.GetClientByLogin(User.Identity.Name);

                ViewData["dateOfBirth"] = client.DateOfBirth.Value.ToString("yyyy-MM-dd");
                ViewData["weight"] = SetLastWeightMeasurement(_context.WeightMeasurement.Where(w => w.ClientId == client.ClientId).ToList());
                ViewData["growth"] = client.Growth;
                ViewData["changeGoal"] = (int)client.WeightChangeGoal;
                ViewData["activity"] = (int)client.ActivityLevel;
                ViewData["pace"] = client.PaceOfChanges.ToString().Replace(',', '.');
                ViewData["isFirstLaunch"] = 0;

                SetBoolean(client.Sex, "sex");
                SetBoolean(client.Breakfast, "breakfast");
                SetBoolean(client.Lunch, "lunch");
                SetBoolean(client.Dinner, "dinner");
                SetBoolean(client.Dessert, "dessert");
                SetBoolean(client.Snack, "snack");
                SetBoolean(client.Supper, "supper");
            }
            catch   //użytkownik nie podał danych (pierwsze uruchomienie)
            {
                ViewData["dateOfBirth"] = DateTime.Now.ToString("yyyy-MM-dd");
                ViewData["isFirstLaunch"] = 1;
            }
        }

        private double SetLastWeightMeasurement(List<WeightMeasurement> measurementList)
        {

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


        private DateTime? ProtectionAgainstTheMinimum(List<WeightMeasurement> measurementList)
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


        private void SetBoolean(bool? flag, string viewDataName)
        {
            if ((bool)flag)
            {
                ViewData[viewDataName] = 1;
            }
            else
            {
                ViewData[viewDataName] = 0;
            }
        }


        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Settings(SettingsModel m, int isFirstLaunch)
        {
            if (ModelState.IsValid)
            {
                var client = SetClientGoals(m, _clientRepository.GetClientByLogin(User.Identity.Name));

                if (isFirstLaunch == 1) { client.DateOfJoining = DateTime.Now.Date; }
                SetDataForDaysFromToday(m, client);
                SetClientWeightMeasurement(m, SetClientData(m, client));
                
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Start", "Home", new { id = client.ClientId });
                }
                catch
                {
                    ModelState.AddModelError("", "Data urodzenia nie może być mniejsza niż 01.01.1900r.");
                    return View(m);
                }

            }

            return View(m);
        }

        private Client SetClientGoals(SettingsModel m, Client client)
        {
            double pace = 0.4;
            try { pace = double.Parse(m.PaceOfChange.Replace('.', ',').Replace(" ", "")); } catch { }

            int caloricDemand = service.CountCaloricDemand(m.Sex, m.Date_of_birth, m.Growth, m.Weight, service.ActivityLevel(m.LevelOfActivity));
            int calorieTarget = service.CountCalorieTarget(caloricDemand, m.WeightChange_Goal, pace);
            int proteinTarget = service.CountProteinTarget(m.Weight, calorieTarget, m.LevelOfActivity);
            int fatTarget = service.CountFatTarget(calorieTarget);
            int carbsTarget = service.CountCarbsTarget(calorieTarget, proteinTarget, fatTarget);

            client.CalorieGoal = calorieTarget;
            client.CaloricDemand = caloricDemand;
            client.ProteinTarget = proteinTarget;
            client.FatTarget = fatTarget;
            client.CarbsTarget = carbsTarget;
            client.PaceOfChanges = pace;

            return client;
        }

        private List<int> GetListOfDaysIDFromToday(Client client)
        {
            List<int> listOfIDDaysFromToday = new List<int>();
            foreach (var item in _context.Day.Where(d => d.ClientId == client.ClientId))
            {
                if (item.Date >= DateTime.Now.Date)
                {
                    listOfIDDaysFromToday.Add(item.DayId);
                }
            }

            return listOfIDDaysFromToday;
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

        private void SetClientWeightMeasurement(SettingsModel m, Client client)
        {
            client.WeightMeasurement.Add(new WeightMeasurement()
            {
                DateOfMeasurement = DateTime.Now,
                Weight = (double)m.Weight,

            });

            _context.Update(client);
        }


    }
}

