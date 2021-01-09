using FitnessApp.Models;
using FitnessApp.Repository.RepoInterface;
using FitnessApp.Repository;
using FitnessApp.Strategy.StrategyInterface;
using System.Linq;

namespace FitnessApp.Strategy.StrategyImpl.DayOfWeekDietStrategyImpl
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
