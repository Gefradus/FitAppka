using FitAppka.Models;
using System.Threading.Tasks;

namespace FitAppka.Service
{
    public interface ISettingsService
    {
        public void ChangeSettings(SettingsModel m, int isFirstLaunch);
    }
}
