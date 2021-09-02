using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SP_SanHtarWebPage.cls
{
    public class clsUtility
    {
        public string SavePathFile(string fileName, string tabName, string imgbit, IHostingEnvironment _hostingEnvironment)
        {
            string pathFile = "";
            string genFileName = DateTime.Now.ToString("ddMMyyyyHHmmss") + fileName;
            pathFile = @"\Dropzone\" + tabName + "\\" + genFileName;
            //string contentRootPath = _hostingEnvironment.ContentRootPath;
            string contentRootPath = _hostingEnvironment.WebRootPath;
            var path = contentRootPath + pathFile;
            Byte[] bytes = Convert.FromBase64String(imgbit);
            System.IO.File.WriteAllBytes(path, bytes);
            pathFile = "Dropzone/" + tabName + "/" + genFileName;
            return pathFile;
        }

        public string SavePathFileGeneral(string fileName, string tabName, string imgbit, IHostingEnvironment _hostingEnvironment)
        {
            string pathFile = "";
            string genFileName = DateTime.Now.ToString("ddMMyyyyHHmmss") + fileName;
            pathFile = @"\Dropzone\" + tabName + "\\" + genFileName;
            //string contentRootPath = _hostingEnvironment.ContentRootPath;
            string contentRootPath = _hostingEnvironment.WebRootPath;
            var path = contentRootPath.Replace("D:", "") + pathFile;
            Byte[] bytes = Convert.FromBase64String(imgbit);
            System.IO.File.WriteAllBytes(path, bytes);
            pathFile = "Dropzone/" + tabName + "/" + genFileName;
            return pathFile;
        }
    }
}
