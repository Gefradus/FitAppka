using FitAppka.Models;
using System.Threading.Tasks;

namespace FitAppka.Service
{
    public interface IClientManageService
    {
        public Task AddNewClient(RegisterModel model);
        public string GetClientLoginFromModel(Client model);
        public bool CheckIfPassCorrect(Client model);
        public bool HasUserAccess(int dayID);

    }
}
