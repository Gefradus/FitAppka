using FitAppka.Models;
using System.Collections.Generic;

namespace FitAppka.Repository.RepoInterface
{
    public interface IDietRepository
    {
        Diet GetDiet(int id);
        IEnumerable<Diet> GetAllDiets();
        List<Diet> GetLoggedInClientDiets();
        Diet Add(Diet diet);
        Diet Update(Diet diet);
        Diet Delete(int id);
    }
}
