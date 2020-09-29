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
            bool isExpress = string.Compare(Process.GetCurrentProcess().ProcessName, "iisexpress") == 0;
            return isExpress ? string.Empty : "/" + Path.GetFileName(_env.WebRootPath.Replace("\\wwwroot", ""));
        }
    }
}
