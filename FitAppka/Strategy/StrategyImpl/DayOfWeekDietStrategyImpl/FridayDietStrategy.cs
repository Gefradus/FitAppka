using System.Linq;
using FitnessApp.Models;
using FitnessApp.Repository;
using FitnessApp.Repository.RepoInterface;
using FitnessApp.Strategy.StrategyInterface;

namespace FitnessApp.Strategy.StrategyImpl.DayOfWeekDietStrategyImpl
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
