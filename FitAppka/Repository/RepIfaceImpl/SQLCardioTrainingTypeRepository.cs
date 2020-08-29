using FitAppka.Models;
using System.Collections.Generic;
using System.Linq;

namespace FitAppka.Repository.RepIfaceImpl
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
            CardioTrainingType cardioType = GetCardioTrainingType(id);

            if (cardioType != null)
            {
                _context.CardioTrainingType.Remove(cardioType);
                _context.SaveChanges();
            }

            return cardioType;
        }

        public IEnumerable<CardioTrainingType> GetAllCardioTrainingTypes()
        {
            return _context.CardioTrainingType.ToList();
        }

        public CardioTrainingType GetCardioTrainingType(int id)
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
