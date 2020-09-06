using FitAppka.Models;

namespace FitAppka.Service
{
    public interface IClientManageService
    {
        public void AddNewClient(RegisterModel model);
        public string GetClientLoginFromModel(Client model);
        public bool CheckIfPassCorrect(Client model);
        public bool HasUserAccess(int dayID);

    }
}
