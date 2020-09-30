using FitAppka.Models;

namespace FitAppka.Service
{
    public interface ISettingsService
    {
        public void ChangeSettings(SettingsDTO m, int isFirstLaunch);
    }
}
