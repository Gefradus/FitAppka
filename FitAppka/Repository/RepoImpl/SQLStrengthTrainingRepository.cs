using FitnessApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;


namespace FitnessApp.Repository.RepIfaceImpl
{
    public class SQLStrengthTrainingRepository : IStrengthTrainingRepository
    {
        private readonly FitAppContext _context;

        public SQLStrengthTrainingRepository(FitAppContext context) {
            _context = context;
        }

        public StrengthTraining Add(StrengthTraining strengthTraining) {
            _context.Add(strengthTraining);
            _context.SaveChanges();
            return strengthTraining;
        }

        public StrengthTraining Delete(int id) {
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
            return _context.StrengthTraining.Include(s => s.StrengthTrainingType).ToList();
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
