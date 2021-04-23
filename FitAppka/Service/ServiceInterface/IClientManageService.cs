using FitnessApp.Models;
using FitnessApp.Models.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FitnessApp.Service
{
    public interface IClientManageService
    {
        Task AddNewClient(RegisterDTO model);
        string GetClientLoginFromModel(LoginDTO model);
        bool CheckIfClientFromModelIsBanned(LoginDTO model);
        List<Client> GetAllClientsAndSortByAdminAndBanned();
        bool ExistsClientByEmail(string email);
        bool ExistsClientByLogin(string login);
        bool CheckIfPassCorrect(LoginDTO model);
        bool HasUserAccessToDay(int dayID);
        bool HasUserAccessToProduct(int productID);
        bool HasUserAccessToDiet(int dietID);
        bool HasUserAccessToWeightMeasurement(int id);
        bool IsLoggedInClientAdmin();
        int GetLoggedInClientId();
        Client GetLoggedInClient();
        
    }
}
