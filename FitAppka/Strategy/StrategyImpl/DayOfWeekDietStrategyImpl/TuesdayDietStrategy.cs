using FitAppka.Models;
using FitAppka.Repository.RepoInterface;
using FitAppka.Repository;
using FitAppka.Strategy.StrategyInterface;
using System.Linq;

namespace FitAppka.Strategy.StrategyImpl.DayOfWeekDietStrategyImpl
{
    public class TuesdayDietStrategy : IDayOfWeekDietStrategy
    {
        public IDietRepository DietRepository { private get; set; }
        public IClientRepository ClientRepository { private get; set; }

        public Diet GetActiveDiet()
        {
            return DietRepository.GetAllDiets().FirstOrDefault(d => d.Active && d.Tuesday && d.IsDeleted == false && d.ClientId == ClientRepository.GetLoggedInClientId());
        }
    }
}
