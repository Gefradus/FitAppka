using FitnessApp.Models;

namespace FitnessApp.Service
{
    public interface ISettingsService
    {
        public void ChangeSettings(SettingsDTO m, int isFirstLaunch, int clientId);
    }
}
