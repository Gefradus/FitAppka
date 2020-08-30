using FitAppka.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitAppka.Repository.RepIfaceImpl
{
    public class SQLCardioTrainingRepository : ICardioTrainingRepository
    {
        private readonly FitAppContext _context;
        public SQLCardioTrainingRepository(FitAppContext context)
        {
            _context = context;
        }

        public CardioTraining Add(CardioTraining cardioTraining)
        {
            _context.Add(cardioTraining);
            _context.SaveChanges();
            return cardioTraining;
        }

        public CardioTraining Delete(int id)
        {
            CardioTraining cardioTraining = GetCardioTraining(id);

            if (cardioTraining != null)
            {
                _context.CardioTraining.Remove(cardioTraining);
                _context.SaveChanges();
            }

            return cardioTraining;
        }

        public IEnumerable<CardioTraining> GetAllCardioTrainings()
        {
            return _context.CardioTraining.ToList();
        }

        public Task<List<CardioTraining>> GetAllCardioTrainingsAsync()
        {
            return _context.CardioTraining.ToListAsync();
        }

        public CardioTraining GetCardioTraining(int id)
        {
            return _context.CardioTraining.Find(id);
        }

        public CardioTraining Update(CardioTraining cardioTraining)
        {
            _context.Update(cardioTraining);
            _context.SaveChanges();
            return cardioTraining;
        }
    }
}
