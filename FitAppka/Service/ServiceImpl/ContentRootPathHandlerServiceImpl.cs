using Microsoft.AspNetCore.Hosting;
using System.Diagnostics;
using System.IO;

namespace FitAppka.Service.ServiceImpl
{
    public class ContentRootPathHandlerServiceImpl : IContentRootPathHandlerService
    {
        private readonly IWebHostEnvironment _env;

        public ContentRootPathHandlerServiceImpl(IWebHostEnvironment env) {
            _env = env;
        }

        public string GetContentRootFileName()
        {
            try {
                Process process = Process.GetCurrentProcess();
                if (process != null) {
                    bool isExpress = string.Compare(process.ProcessName, "iisexpress") == 0;
                    return isExpress ? string.Empty : "/" + Path.GetFileName(_env.WebRootPath.Replace("\\wwwroot", ""));
                }
                return "";
            }
            catch {
                return string.Empty;
            }
        }
    }
}
