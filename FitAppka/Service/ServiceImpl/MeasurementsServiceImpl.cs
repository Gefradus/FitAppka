using FitAppka.Model;
using FitAppka.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FitAppka.Service.ServiceImpl
{
    public class MeasurementsServiceImpl : IMeasurementsService
    {
        private readonly IWeightMeasurementRepository _weightMeasurementRepository;
        private readonly IFatMeasurementRepository _fatMeasurementRepository;
        private readonly IClientManageService _clientManageService;
        private readonly IClientRepository _clientRepository;

        public MeasurementsServiceImpl(IWeightMeasurementRepository weightMeasurementRepository, IFatMeasurementRepository fatMeasurementRepository,
            IClientManageService clientManageService, IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
            _clientManageService = clientManageService;
            _weightMeasurementRepository = weightMeasurementRepository;
            _fatMeasurementRepository = fatMeasurementRepository;
        }

        private FatMeasurement AddFatMeasurementIfWaistNotZero(WeightMeasurement weightMeasurement, short weight, int waist)
        {
            FatMeasurement fatMeasurement = CreateFatMeasurementIfWaistNotZero(weightMeasurement, weight, waist);
            if(fatMeasurement != null)
            {
                return _fatMeasurementRepository.Add(fatMeasurement);
            }
            return null;
        }

        public bool DeleteMeasurement(int id)
        {
            if (_clientManageService.HasUserAccessToWeightMeasurement(id)) {
                List<FatMeasurement> relatedFatMeasaurement = _weightMeasurementRepository.GetWeightMeasurement(id).FatMeasurement.ToList();
                foreach(var item in relatedFatMeasaurement) {
                    _fatMeasurementRepository.Delete(item.FatMeasurementId);
                }
                _weightMeasurementRepository.Delete(id);
                return true;
            }
            return false;
        }

        public double EstimateBodyFatLevel(short weight, int waist)
        {
            double var;
            if ((bool) _clientRepository.GetLoggedInClient().Sex) {
                var = 98.42;
            }
            else {
                var = 76.76;
            }

            return FormatBodyFatLevel(((4.15 * waist / 2.54) - (0.082 * weight * 2.2) - var) / (weight * 2.2) * 100);   //YMCA Method
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
                ClientId = _clientRepository.GetLoggedInClientId(),
                Weight = weight,
                DateOfMeasurement = DateTime.Now
            };
        }

        private FatMeasurement CreateFatMeasurementIfWaistNotZero(WeightMeasurement weightMeasurement, short weight, int waist)
        {
            if (waist != 0) {
                return new FatMeasurement() {
                    WeightMeasurementId = weightMeasurement.WeightMeasurementId,
                    BodyFatLevel = EstimateBodyFatLevel(weight, waist),
                    WaistCircumference = waist
                };
            }
            return null;
        }

        private void UpdateOrAddFatMeasurementIfExist(WeightMeasurement item, short weight, int waist)
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
                else {
                    AddFatMeasurementAndUpdateWeightMeasurement(item, weight, waist);
                }
            }
        }

        public bool AddOrUpdateMeasurements(short weight, int waist)
        {
            WeightMeasurement createdMeasurement = CreateWeightMeasurement(weight);
            foreach (var item in _weightMeasurementRepository.GetClientsWeightMeasurements(_clientRepository.GetLoggedInClientId())) {
                if (item.DateOfMeasurement.Value.Date == createdMeasurement.DateOfMeasurement.Value.Date) {
                    item.Weight = createdMeasurement.Weight;
                    UpdateOrAddFatMeasurementIfExist(item, weight, waist);
                    return UpdateWeightMeasurement(item);
                }
            }

            AddFatMeasurementAndUpdateWeightMeasurement(_weightMeasurementRepository.Add(createdMeasurement), weight, waist);
            return true;
        }

        public bool UpdateMeasurements(int id, short weight, int waist){
            WeightMeasurement weightMeasurement = _weightMeasurementRepository.GetWeightMeasurement(id);
            weightMeasurement.Weight = weight;
            UpdateWeightMeasurement(weightMeasurement);
            UpdateOrAddFatMeasurementIfExist(weightMeasurement, weight, waist);
            return true;
        }

        private void AddFatMeasurementAndUpdateWeightMeasurement(WeightMeasurement addedMeasurement, short weight, int waist)
        {
            FatMeasurement fatMeasurement = AddFatMeasurementIfWaistNotZero(addedMeasurement, weight, waist);
            if (fatMeasurement != null) {
                addedMeasurement.FatMeasurementId = fatMeasurement.FatMeasurementId;
                UpdateWeightMeasurement(addedMeasurement);
            }
        }
    }
}
