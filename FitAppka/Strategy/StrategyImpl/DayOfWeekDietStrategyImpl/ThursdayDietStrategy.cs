using FitAppka.Models;
using FitAppka.Repository.RepoInterface;
using FitAppka.Strategy.StrategyInterface;
using System.Linq;
using FitAppka.Repository;

namespace FitAppka.Strategy.StrategyImpl.DayOfWeekDietStrategyImpl
{

    public class ThursdayDietStrategy : IDayOfWeekDietStrategy
    {
        public IDietRepository DietRepository { private get; set; }
        public IClientRepository ClientRepository { private get; set; }

        public Diet GetActiveDiet()
        {
            return DietRepository.GetAllDiets().FirstOrDefault(d => d.Active && d.Thursday && d.IsDeleted == false && d.ClientId == ClientRepository.GetLoggedInClientId());
        }
    }
}
