using FitnessApp.Models;
using FitnessApp.Models.DTO;
using FitnessApp.Models.DTO.DietCreatorDTO;
using System.Collections.Generic;

namespace FitnessApp.Service.ServiceInterface
{
    public interface IAdministrationService
    {
        public bool UnbanClient(int id);
        public bool BanClient(int id);
        public void EditClient(ClientAdministrationDTO dto, int id);
        public ClientAdministrationDTO GetClientAdministrationDTO(int id);
        public bool AddCardioType(string name, int kcalPerMin, bool visibleToAll);
        public bool EditCardioType(int id, string name, int kcalPerMin, bool visibleToAll);
        public bool AddStrengthTrainingType(string name, bool visibleToAll);
        public bool EditStrengthTrainingType(int id, string name, bool visibleToAll);
        public bool DeleteCardioType(int id);
        public bool DeleteStrengthTrainingType(int id);
        public TrainingsDTO GetTrainingsDTO(string searchCardio, string searchStrength);
        public List<AdminDietDTO> GetAdminDiets();
    }
}
