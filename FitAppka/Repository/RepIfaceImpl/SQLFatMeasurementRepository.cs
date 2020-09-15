using FitAppka.Model;
using System.Collections.Generic;
using System.Linq;


namespace FitAppka.Repository.RepIfaceImpl{
    public class SQLFatMeasurementRepository : IFatMeasurementRepository
    {
        private readonly FitAppContext _context;

        public SQLFatMeasurementRepository(FitAppContext context)
        {
            _context = context;
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
    }
}