using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.IO.Compression;
using Ionic.Zip;
using Sungero.Core;
using Akelon.SolutionStorage.Structures.Module;

namespace Akelon.SolutionStorage.Isolated.ZipHandler
{
  public class ZipHelper
  {
    const string path = "C:\\TempDirectory\\";
    const string fileDatName = "fileDat.dat";
    const string fileXmlName = "fileXml.xml";
    const string fileZipName = "fileZip.zip";
    
    public ZipHelper()
    {
      
    }
    
    public static bool CheckZipInput()
    {
      return true;
    }
    
    public static string CreateZip(string fileDat, string fileXml)
    {
      var zip = new Ionic.Zip.ZipFile(path + fileZipName);
      zip.AddFile(path + fileDatName);
      zip.AddFile(path + fileXmlName);
      // TODO: Could not find a part of the path 'C:\TempDirectory\DotNetZip-pygsb3tt.tmp'.
      zip.Save();
      
      Logger.Debug("-_-_-_-_-_-_Isolated Area - тру");
      return "Success";
    }
  }
}