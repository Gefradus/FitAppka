using FitAppka.Models;

namespace FitAppka.Service.ServiceInterface
{
    public interface IAdministrationService
    {
        public bool UnbanClient(int id);
        public bool BanClient(int id);
        public void EditClient(ClientAdministrationDTO dto, int id);
        public ClientAdministrationDTO GetClientAdministrationDTO(int id); 
    }
}
