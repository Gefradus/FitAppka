using FitnessApp.Models;

namespace FitnessApp.Service
{
    public interface ISettingsService
    {
        public void ChangeSettings(SettingsDTO m, bool isFirstLaunch, int clientId);
        public SettingsDTO Dto();
    }
}
