using FitnessApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FitnessApp.Service
{
    public interface IClientManageService
    {
        Task AddNewClient(RegisterDTO model);
        string GetClientLoginFromModel(Client model);
        bool CheckIfClientFromModelIsBanned(Client model);
        List<Client> GetAllClientsAndSortByAdminAndBanned();
        bool CheckIfPassCorrect(Client model);
        bool HasUserAccessToDay(int dayID);
        bool HasUserAccessToProduct(int productID);
        bool HasUserAccessToDiet(int dietID);
        bool HasUserAccessToWeightMeasurement(int id);
        bool IsLoggedInClientAdmin();
        int GetLoggedInClientId();
        Client GetLoggedInClient();
        
    }
}
