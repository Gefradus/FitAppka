using FitnessApp.Models;
using FitnessApp.Repository.RepoInterface;
using FitnessApp.Strategy.StrategyInterface;
using FitnessApp.Repository;
using System.Linq;

namespace FitnessApp.Strategy.StrategyImpl.DayOfWeekDietStrategyImpl
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
