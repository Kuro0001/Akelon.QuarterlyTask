using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using Ionic.Zip;
using Sungero.Core;
using Akelon.SolutionStorage.Structures.Module;

namespace Akelon.SolutionStorage.Isolated.ZipHandler
{
  //  [Public(Isolated = true)]
  //  public enum FileNames
  //  {
  //    fileDatName = path + "fileDat.dat",
  //    fileXmlName = path + "fileXml.xml",
  //    fileZipName = path + "fileZip.zip"
  //  }
  
  public class ZipHelper
  {
    const string path = @"C:\TempDirectory\";
    const string fileDatName = path + "fileDat.dat";
    const string fileXmlName = path + "fileXml.xml";
    const string fileZipName = path + "fileZip.zip";
    
    public ZipHelper() { }
    
    public static bool CheckZipInput()
    {
      return true;
    }
    
    public static string CreateZip(string fileDat, string fileXml)
    {
      Logger.Debug($"Isolate Dat: {fileDat}");
      Logger.Debug($"Isolate Xml: {fileXml}");
      if (!Directory.Exists(path))
      {
        Directory.CreateDirectory(path);
      }
      
      var zip = new ZipFile(fileZipName);
      
      CreateDefaultFile(fileDat, fileDatName);
      CreateDefaultFile(fileXml, fileXmlName);
      
      zip.AddFile(fileDatName);
      zip.AddFile(fileXmlName);
      zip.Save();
      
      byte[] byteZip = File.ReadAllBytes(fileZipName);
      
      Directory.Delete(path, true);
      
      return Encoding.Default.GetString(byteZip);
    }
    
    public static void CreateDefaultFile(string file, string name)
    {
      try
      {
        using (FileStream fs = File.Create(name))
        {
          byte[] info = new UTF8Encoding(true).GetBytes(file);
          fs.Write(info, 0, info.Length);
        }
      }
      catch
      {
        throw AppliedCodeException.Create("Create default file error!");
      }
    }
  }
}