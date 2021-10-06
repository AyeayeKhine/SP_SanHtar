using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SP_SanHtar.Web.cls
{
    public class clsUtility
    {
        public string SavePathFile(string fileName, string tabName, string imgbit, IHostingEnvironment _hostingEnvironment,string extension)
        {
            string pathFile = "";
            string genFileName = fileName + DateTime.Now.ToString("ddMMyyyyHHmmss")+extension ;
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

        public async  Task<byte[]> FileToByteArray(string filePath)
        {
             byte[] fileData = null;
            string pathfile = filePath;
            FileStream fs = new FileStream(pathfile, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            fileData = br.ReadBytes((int)fs.Length);
            br.Close();
            fs.Close();
            return  fileData;
        }

        public static bool CheckNotNull(object item)
        {
            if(item != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool CheckNull(object item)
        {
            if (item == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    // public class Get
    //{
    //    public static string BaseURL()
    //    {
    //        HttpRequest request = HttpContext.Current.Request;
    //        string appUrl = HttpRuntime.AppDomainAppVirtualPath;
    //        if (appUrl != "/") appUrl = "/" + appUrl;
    //        return string.Format("{0}://{1}{2}", request.Url.Scheme, request.Url.Authority, appUrl);
    //    }
    //}
}
