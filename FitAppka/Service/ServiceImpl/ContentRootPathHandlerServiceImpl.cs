using Microsoft.AspNetCore.Hosting;
using System.Diagnostics;
using System.IO;

namespace FitnessApp.Service.ServiceImpl
{
    public class ContentRootPathHandlerServiceImpl : IContentRootPathHandlerService
    {
        private readonly IWebHostEnvironment _env;

        public ContentRootPathHandlerServiceImpl(IWebHostEnvironment env) {
            _env = env;
        }

        public string GetContentRootFileName()
        {
            if (string.Compare(Process.GetCurrentProcess().ProcessName, "w3wp") == 0) {
                return "/" + Path.GetFileName(_env.WebRootPath.Replace("\\wwwroot", ""));
            }
            return "";
        }
    }
}
