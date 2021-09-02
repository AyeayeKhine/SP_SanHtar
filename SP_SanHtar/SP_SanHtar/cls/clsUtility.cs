using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SP_SanHtar.cls
{
  public  class clsUtility
    {
        public byte[] FileToByteArray(string fileName)
        {
            byte[] fileData = null;

            using (FileStream fs = System.IO.File.OpenRead(fileName))
            {
                using (BinaryReader binaryReader = new BinaryReader(fs))
                {
                    fileData = binaryReader.ReadBytes((int)fs.Length);
                    //Convert.FromBase64String(fileData.ToString());
                }
            }
            return fileData;
        }
    }
}
