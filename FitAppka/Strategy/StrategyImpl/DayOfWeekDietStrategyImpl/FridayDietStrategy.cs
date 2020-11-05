using System.Linq;
using FitAppka.Models;
using FitAppka.Repository;
using FitAppka.Repository.RepoInterface;
using FitAppka.Strategy.StrategyInterface;

namespace FitAppka.Strategy.StrategyImpl.DayOfWeekDietStrategyImpl
{
    public class FridayDietStrategy : IDayOfWeekDietStrategy
    {
        public IDietRepository DietRepository { private get; set; }
        public IClientRepository ClientRepository { private get; set; }

        public Diet GetActiveDiet()
        {
            return DietRepository.GetAllDiets().FirstOrDefault(d => d.Active && d.Friday && d.IsDeleted == false && d.ClientId == ClientRepository.GetLoggedInClientId());
        }
    }
}
