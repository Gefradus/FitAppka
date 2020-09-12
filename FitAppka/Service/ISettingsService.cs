using FitAppka.Models;
using System.Threading.Tasks;

namespace FitAppka.Service
{
    public interface ISettingsService
    {
        public Task ChangeSettings(SettingsModel m, int isFirstLaunch);
    }
}
