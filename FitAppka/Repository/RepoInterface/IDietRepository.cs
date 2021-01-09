using FitnessApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FitnessApp.Repository.RepoInterface
{
    public interface IDietRepository
    {
        Diet GetDiet(int id);
        IEnumerable<Diet> GetAllDiets();
        List<Diet> GetLoggedInClientDiets();
        Task<List<Diet>> GetActiveDiets();
        Diet Add(Diet diet);
        Diet Update(Diet diet);
        Diet Delete(int id);
    }
}
