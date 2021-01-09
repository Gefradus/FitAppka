using FitnessApp.Models;
namespace FitnessApp.Service
{
    public interface IFindDayService
    {
        public FindDayDTO FindDays(FindDayDTO findDayDTO);
    }
}
