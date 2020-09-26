using FitAppka.Models;
using System;
using System.Collections.Generic;
using System.Linq;


namespace FitAppka.Repository.RepIfaceImpl
{
    public class SQLWeightMeasurementRepository : IWeightMeasurementRepository
    {
        private readonly FitAppContext _context;
        private readonly IClientRepository _clientRepository;

        public SQLWeightMeasurementRepository(FitAppContext context, IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
            _context = context;
        }

        public WeightMeasurement Add(WeightMeasurement weightMeasurement)
        {
            _context.Add(weightMeasurement);
            _context.SaveChanges();
            return weightMeasurement;
        }

        public WeightMeasurement Delete(int id)
        {
            WeightMeasurement weightMeasurement = GetWeightMeasurement(id);

            if (weightMeasurement != null)
            {
                _context.WeightMeasurement.Remove(weightMeasurement);
                _context.SaveChanges();
            }

            return weightMeasurement;
        }

        public IEnumerable<WeightMeasurement> GetAllWeightMeasurements()
        {
            return _context.WeightMeasurement.ToList();
        }

        public List<WeightMeasurement> GetLoggedInClientWeightMeasurements()
        {
            return _context.WeightMeasurement.Where(w => w.ClientId == _clientRepository.GetLoggedInClientId()).ToList();
        }

        public WeightMeasurement GetWeightMeasurement(int id)
        {
            return _context.WeightMeasurement.Find(id);
        }

        public WeightMeasurement Update(WeightMeasurement weightMeasurement)
        {
            _context.Update(weightMeasurement);
            _context.SaveChanges();
            return weightMeasurement;
        }

        public double GetLastLoggedInClientWeight()
        {
            var measurementList = GetLoggedInClientWeightMeasurements();

            double lastWeightMeasurement = 0;
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
    }
}
