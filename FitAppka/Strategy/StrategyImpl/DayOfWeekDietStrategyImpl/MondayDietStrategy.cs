using FitAppka.Models;
using FitAppka.Repository.RepoInterface;
using FitAppka.Strategy.StrategyInterface;
using FitAppka.Repository;
using System.Linq;

namespace FitAppka.Strategy.StrategyImpl.DayOfWeekDietStrategyImpl
{
    public class MondayDietStrategy : IDayOfWeekDietStrategy
    {
        public IDietRepository DietRepository { private get; set; }
        public IClientRepository ClientRepository { private get; set; }

        public Diet GetActiveDiet()
        {
            return DietRepository.GetAllDiets().FirstOrDefault(d => d.Active && d.Monday && d.IsDeleted == false && d.ClientId == ClientRepository.GetLoggedInClientId());
        }
    }
}
