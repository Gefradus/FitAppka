using System.Linq;
using FitAppka.Models;
using FitAppka.Repository.RepoInterface;
using FitAppka.Strategy.StrategyInterface;
using FitAppka.Repository;

namespace FitAppka.Strategy.StrategyImpl.DayOfWeekDietStrategyImpl
{
    public class WednesdayDietStrategy : IDayOfWeekDietStrategy
    {
        public IDietRepository DietRepository { private get; set; }
        public IClientRepository ClientRepository { private get; set; }

        public Diet GetActiveDiet()
        {
            return DietRepository.GetAllDiets().FirstOrDefault(d => d.Active && d.Wednesday && d.IsDeleted == false && d.ClientId == ClientRepository.GetLoggedInClientId());
        }
    }
}
