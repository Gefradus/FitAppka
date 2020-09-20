using FitAppka.Models;
using System.Threading.Tasks;

namespace FitAppka.Service
{
    public interface IClientManageService
    {
        Task AddNewClient(RegisterDTO model);
        string GetClientLoginFromModel(Client model);
        bool CheckIfPassCorrect(Client model);
        bool HasUserAccessToDay(int dayID);
        bool HasUserAccessToProduct(int productID);
        bool HasUserAccessToWeightMeasurement(int id);
        bool IsLoggedInClientAdmin();
        int GetLoggedInClientId();
        Client GetLoggedInClient();
        
    }
}
