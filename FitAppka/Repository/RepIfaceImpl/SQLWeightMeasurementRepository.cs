using FitAppka.Models;
using System.Collections.Generic;
using System.Linq;


namespace FitAppka.Repository.RepIfaceImpl
{
    public class SQLWeightMeasurementRepository : IWeightMeasurementRepository
    {
        private readonly FitAppContext _context;
        public SQLWeightMeasurementRepository(FitAppContext context)
        {
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
    }
}
