using FitnessApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessApp.Repository.RepIfaceImpl
{
    public class SQLCardioTrainingTypeRepository : ICardioTrainingTypeRepository
    {
        private readonly FitAppContext _context;

        public SQLCardioTrainingTypeRepository(FitAppContext context)
        {
            _context = context;
        }

        public CardioTrainingType Add(CardioTrainingType cardioType)
        {
            _context.Add(cardioType);
            _context.SaveChanges();
            return cardioType;
        }

        public CardioTrainingType Delete(int id)
        {
            CardioTrainingType cardioType = GetCardioType(id);

            if (cardioType != null)
            {
                GetCardioType(id).IsDeleted = true;
                _context.SaveChanges();
            }

            return cardioType;
        }

        public IEnumerable<CardioTrainingType> GetAllCardioTypes(string search)
        {
            return _context.CardioTrainingType.Where(s => s.IsDeleted == false && (s.TrainingName.Contains(search) || string.IsNullOrWhiteSpace(search))).ToList();
        }

        public async Task<List<CardioTrainingType>> GetAllCardioTypesAsync(string search)
        {
            return await _context.CardioTrainingType.Where(s => s.IsDeleted == false && s.TrainingName.Contains(search)).ToListAsync();
        }

        public CardioTrainingType GetCardioType(int id)
        {
            return _context.CardioTrainingType.Find(id);
        }

        public CardioTrainingType Update(CardioTrainingType cardioType)
        {
            _context.Update(cardioType);
            _context.SaveChanges();
            return cardioType;
        }
    }
}
