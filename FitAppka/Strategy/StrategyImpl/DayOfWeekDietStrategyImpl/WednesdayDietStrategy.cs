using System.Linq;
using FitnessApp.Models;
using FitnessApp.Repository.RepoInterface;
using FitnessApp.Strategy.StrategyInterface;
using FitnessApp.Repository;

namespace FitnessApp.Strategy.StrategyImpl.DayOfWeekDietStrategyImpl
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
