using FitAppka.Models;

namespace FitAppka.Service
{
    public interface ISettingsService
    {
        public void ChangeSettings(SettingsModel m, int isFirstLaunch);
        public double SetLastWeightMeasurement();

    }
}
