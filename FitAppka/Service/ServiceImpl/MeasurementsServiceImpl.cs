using FitAppka.Models;
using FitAppka.Repository;
using System;

namespace FitAppka.Service.ServiceImpl
{
    public class MeasurementsServiceImpl : IMeasurementsService
    {
        private readonly IWeightMeasurementRepository _weightMeasurementRepository;
        private readonly IFatMeasurementRepository _fatMeasurementRepository;
        private readonly IClientManageService _clientManageService;
        private readonly IGoalsService _goalsService;

        public MeasurementsServiceImpl(IWeightMeasurementRepository weightMeasurementRepository, IGoalsService goalsService,
            IFatMeasurementRepository fatMeasurementRepository, IClientManageService clientManageService)
        {
            _goalsService = goalsService;
            _clientManageService = clientManageService;
            _weightMeasurementRepository = weightMeasurementRepository;
            _fatMeasurementRepository = fatMeasurementRepository;
        }

        public void AddMeasurements(short weight, int? waist)
        {
            AddFatMeasurementAndUpdateWeightMeasurement(_weightMeasurementRepository.Add(CreateWeightMeasurement(weight)), weight, waist);
            UpdateGoals();
        }

        public bool UpdateMeasurements(int id, short weight, int? waist)
        {
            WeightMeasurement weightMeasurement = _weightMeasurementRepository.GetWeightMeasurement(id);
            weightMeasurement.Weight = weight;
            UpdateWeightMeasurement(weightMeasurement);
            UpdateOrAddFatMeasurement(weightMeasurement, weight, waist);
            UpdateGoals();
            return true;
        }

        public bool DeleteMeasurement(int id)
        {
            if (_clientManageService.HasUserAccessToWeightMeasurement(id) && _weightMeasurementRepository.GetLoggedInClientWeightMeasurements().Count > 1)
            {
                int? relatedFatMeasurementId = _weightMeasurementRepository.GetWeightMeasurement(id).FatMeasurementId;
                if (relatedFatMeasurementId != null) {
                    _fatMeasurementRepository.Delete((int)relatedFatMeasurementId);
                }
                
                _weightMeasurementRepository.Delete(id);
                UpdateGoals();
                return true;
            }
            return false;
        }

        public double EstimateBodyFatLevel(short weight, int? waist)
        {
            double var;
            if ((bool) _clientManageService.GetLoggedInClient().Sex) {
                var = 98.42;
            }
            else {
                var = 76.76;
            }

            return FormatBodyFatLevel((double)(((4.15 * waist / 2.54) - (0.082 * weight * 2.2) - var) / (weight * 2.2) * 100));   //YMCA Method
        }

        private double FormatBodyFatLevel(double fatLevel){
            double var = RoundDouble(fatLevel);
            if(var > 100){
                return 100;
            }
            else if(var < 0){
                return 0;
            }
            else {
                return var;
            }
        }

        private double RoundDouble(double number)
        {
            return (double)Math.Round((decimal)number, 1, MidpointRounding.AwayFromZero);
        }

        private FatMeasurement UpdateFatMeasurement(FatMeasurement fatMeasurement)
        {
            if (_clientManageService.HasUserAccessToWeightMeasurement(fatMeasurement.WeightMeasurementId))
            {
                return _fatMeasurementRepository.Update(fatMeasurement);
            }
            return null;
        }

        private bool UpdateWeightMeasurement(WeightMeasurement weightMeasurement)
        {
            if (_clientManageService.HasUserAccessToWeightMeasurement(weightMeasurement.WeightMeasurementId)) 
            {
                _weightMeasurementRepository.Update(weightMeasurement);
                return true;
            }
            return false;
        }

        private WeightMeasurement CreateWeightMeasurement(short weight)
        {
            return new WeightMeasurement() {
                ClientId = _clientManageService.GetLoggedInClientId(),
                Weight = weight,
                DateOfMeasurement = DateTime.Now
            };
        }

        private FatMeasurement CreateFatMeasurementIfWaistNotZero(WeightMeasurement weightMeasurement, short weight, int? waist)
        {
            if (waist != 0 && waist != null) {
                return new FatMeasurement() {
                    WeightMeasurementId = weightMeasurement.WeightMeasurementId,
                    BodyFatLevel = EstimateBodyFatLevel(weight, waist),
                    WaistCircumference = waist
                };
            }
            return null;
        }

        private void UpdateOrAddFatMeasurement(WeightMeasurement item, short weight, int? waist)
        {
            if (item.FatMeasurementId != 0)
            {
                if(item.FatMeasurementId != null)
                {
                    FatMeasurement fatMeasurement = _fatMeasurementRepository.GetFatMeasurement((int)item.FatMeasurementId);
                    fatMeasurement.WaistCircumference = waist;
                    fatMeasurement.BodyFatLevel = EstimateBodyFatLevel(weight, waist);
                    UpdateFatMeasurement(fatMeasurement);
                }
                else 
                {
                    AddFatMeasurementAndUpdateWeightMeasurement(item, weight, waist);
                }
            }
        }

        private FatMeasurement AddFatMeasurementIfWaistNotZero(WeightMeasurement weightMeasurement, short weight, int? waist)
        {
            FatMeasurement fatMeasurement = CreateFatMeasurementIfWaistNotZero(weightMeasurement, weight, waist);
            if (fatMeasurement != null)
            {
                return _fatMeasurementRepository.Add(fatMeasurement);
            }
            return null;
        }

        private void AddFatMeasurementAndUpdateWeightMeasurement(WeightMeasurement addedMeasurement, short weight, int? waist)
        {
            FatMeasurement fatMeasurement = AddFatMeasurementIfWaistNotZero(addedMeasurement, weight, waist);
            if (fatMeasurement != null) {
                addedMeasurement.FatMeasurementId = fatMeasurement.FatMeasurementId;
                UpdateWeightMeasurement(addedMeasurement);
            }
        }

        private void UpdateGoals()
        {
            _goalsService.SetClientGoalsIfAutoDietaryGoals((bool)_clientManageService.GetLoggedInClient().AutoDietaryGoals);
            _goalsService.UpdateGoalsInDaysFromToday();
        }


    }
}
