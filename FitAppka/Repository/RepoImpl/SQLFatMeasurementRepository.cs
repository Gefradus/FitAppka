using FitAppka.Models;
using System.Collections.Generic;
using System.Linq;


namespace FitAppka.Repository.RepIfaceImpl{
    public class SQLFatMeasurementRepository : IFatMeasurementRepository
    {
        private readonly FitAppContext _context;
        private readonly IClientRepository _clientRepository;

        public SQLFatMeasurementRepository(FitAppContext context, IClientRepository clientRepository)
        {
            _context = context;
            _clientRepository = clientRepository;
        }

        public FatMeasurement GetFatMeasurement(int id){
            return _context.FatMeasurement.Find(id);
        }
        public IEnumerable<FatMeasurement> GetAllFatMeasurements(){
            return _context.FatMeasurement.ToList();
        }


        public FatMeasurement Add(FatMeasurement fatMeasurement) {
            _context.Add(fatMeasurement);
            _context.SaveChanges();
            return fatMeasurement;
        }

        public FatMeasurement Update(FatMeasurement fatMeasurement) {
            _context.Update(fatMeasurement);
            _context.SaveChanges();
            return fatMeasurement;
        }   

        public FatMeasurement Delete(int id) {
            FatMeasurement fatMeasurement = GetFatMeasurement(id);
            if(fatMeasurement != null){
                _context.FatMeasurement.Remove(fatMeasurement);
                _context.SaveChanges();
            }
            
            return fatMeasurement;
        }

        public IEnumerable<FatMeasurement> GetLoggedInClientFatMeasurements()
        {
            var fatMeasurements = new List<FatMeasurement>();
            foreach (var fatMeasurement in GetAllFatMeasurements())
            {
                if(_context.WeightMeasurement.FirstOrDefault(w => w.FatMeasurementId == fatMeasurement.FatMeasurementId)
                    .ClientId  == _clientRepository.GetLoggedInClientId())
                {
                    fatMeasurements.Add(fatMeasurement);
                }
            }

            return fatMeasurements;
        }
    }
}