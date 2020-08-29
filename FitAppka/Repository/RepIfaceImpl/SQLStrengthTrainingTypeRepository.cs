using FitAppka.Models;
using System.Collections.Generic;
using System.Linq;


namespace FitAppka.Repository.RepIfaceImpl
{
    public class SQLStrengthTrainingTypeRepository : IStrengthTrainingTypeRepository
    {
        private readonly FitAppContext _context;

        public SQLStrengthTrainingTypeRepository(FitAppContext context)
        {
            _context = context;
        }

        public StrengthTrainingType Add(StrengthTrainingType strengthTrainingType)
        {
            _context.Add(strengthTrainingType);
            _context.SaveChanges();
            return strengthTrainingType;
        }

        public StrengthTrainingType Delete(int id)
        {
            StrengthTrainingType strengthTrainingType = GetStrengthTrainingType(id);

            if (strengthTrainingType != null)
            {
                _context.StrengthTrainingType.Remove(strengthTrainingType);
                _context.SaveChanges();
            }

            return strengthTrainingType;
        }

        public IEnumerable<StrengthTrainingType> GetAllStrengthTrainingTypes()
        {
            return _context.StrengthTrainingType.ToList();
        }

        public StrengthTrainingType GetStrengthTrainingType(int id)
        {
            return _context.StrengthTrainingType.Find(id);
        }

        public StrengthTrainingType Update(StrengthTrainingType strengthTrainingType)
        {
            _context.Update(strengthTrainingType);
            _context.SaveChanges();
            return strengthTrainingType;
        }
    }
}
