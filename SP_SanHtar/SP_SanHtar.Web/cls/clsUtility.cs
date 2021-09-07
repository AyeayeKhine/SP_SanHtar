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

        public async  Task<byte[]> FileToByteArray(string filePath)
        {
             byte[] fileData = null;
            //using var httpClient = new HttpClient();
            //FileInfo fi = new FileInfo(filePath);
            //using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
            //{
            //    using (BinaryReader binaryReader = new BinaryReader(fs))
            //    {
            //        fileData = binaryReader.ReadBytes((int)fs.Length);
            //        //Convert.FromBase64String(fileData.ToString());
            //    }
            //}
            //fileData = File.ReadAllBytes(filePath);
            string pathfile = filePath;
            FileStream fs = new FileStream(pathfile, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);

            fileData = br.ReadBytes((int)fs.Length);

            br.Close();
            fs.Close();
            //fileData = await httpClient.GetByteArrayAsync(filePath);
            return fileData;
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
