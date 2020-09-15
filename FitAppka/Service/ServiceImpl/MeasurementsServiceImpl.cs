using FitAppka.Model;
using FitAppka.Repository;
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

        public bool AddOrUpdateMeasurement(WeightMeasurement weightMeasurement)
        {
            int clientId = _clientRepository.GetLoggedInClientId();
            if (weightMeasurement.ClientId == clientId)
            {
                var weightMeasurements = _weightMeasurementRepository.GetClientsWeightMeasurements(clientId);
                _weightMeasurementRepository.Add(weightMeasurement);
                return true;
            }
            return false;
        }

        public bool DeleteMeasurement(int id)
        {
            if (_clientManageService.HasUserAccessToWeightMeasurement(id))
            {
                List<FatMeasurement> relatedFatMeasaurement = _weightMeasurementRepository.GetWeightMeasurement(id).FatMeasurement.ToList();
                foreach(var item in relatedFatMeasaurement) {
                    _fatMeasurementRepository.Delete(item.FatMeasurementId);
                }
                _weightMeasurementRepository.Delete(id);
                return true;
            }
            return false;
        }

        public bool UpdateMeasurement(WeightMeasurement weightMeasurement)
        {
            if (_clientManageService.HasUserAccessToWeightMeasurement(weightMeasurement.WeightMeasurementId))
            {
                _weightMeasurementRepository.Update(weightMeasurement);
                return true;
            }
            return false;
        }

    }
}
