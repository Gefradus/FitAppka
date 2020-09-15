using FitAppka.Model;
using System.Collections.Generic;
using System.Linq;


namespace FitAppka.Repository.RepIfaceImpl
{
    public class SQLStrengthTrainingRepository : IStrengthTrainingRepository
    {
        private readonly FitAppContext _context;

        public SQLStrengthTrainingRepository(FitAppContext context)
        {
            _context = context;
        }

        public StrengthTraining Add(StrengthTraining strengthTraining)
        {
            _context.Add(strengthTraining);
            _context.SaveChanges();
            return strengthTraining;
        }

        public StrengthTraining Delete(int id)
        {
            StrengthTraining strengthTraining = GetStrengthTraining(id);

            if (strengthTraining != null)
            {
                _context.StrengthTraining.Remove(strengthTraining);
                _context.SaveChanges();
            }

            return strengthTraining;
        }

        public IEnumerable<StrengthTraining> GetAllStrengthTrainings()
        {
            return _context.StrengthTraining.ToList();
        }

        public StrengthTraining GetStrengthTraining(int id)
        {
            return _context.StrengthTraining.Find(id);
        }

        public StrengthTraining Update(StrengthTraining strengthTraining)
        {
            _context.Update(strengthTraining);
            _context.SaveChanges();
            return strengthTraining;
        }
    }
}
