using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using Ionic.Zip;
using Sungero.Core;

namespace Akelon.SolutionStorage.Isolated.ZipHandler
{
  
  public class ZipHelper
  {
    const string path = @"C:\TempDirectory\";
    const string fileDatName = path + "fileDat.dat";
    const string fileXmlName = path + "fileXml.xml";
    const string fileZipName = path + "fileZip.zip";
    
    public ZipHelper() { }
    
    /// <summary>
    /// Проверить расширения файлов в zip-архиве на наличие .dat и .xml
    /// </summary>
    /// <param name="fileZip">Zip-файл, включающий в себя пакет решения</param>
    /// <returns>Если в архиве 2 файла и один из них .dat, второй - .xml, то результат = true, иначе - false.</returns>
    public static bool CheckZipInput(Stream fileZip)
    {
      bool isCorrect = false;
      try
      {
        if (!Directory.Exists(path))
        {
          Directory.CreateDirectory(path);
        }
        
        var zip = new ZipFile(fileZipName);
        var fileNames = zip.EntryFileNames;
        if (fileNames.Count != 2)
          isCorrect = CheckFilesExtensions(fileNames);
      }
      catch (Exception ex)
      {
        Logger.ErrorFormat("Isolated area. CheckZipInput. Error: {0}", ex.Message);
      }
      finally
      {
        Directory.Delete(path, true);
        return isCorrect;
      }
    }
    
    /// <summary>
    /// Проверить расширения файлов на наличие .dat и .xml файлов
    /// </summary>
    /// <param name="names">Список наименований файлов в количестве двух (2) штук.</param>
    /// <returns>Если в списке 2 файла и один из них .dat, второй - .xml, то результат = true, иначе - false.</returns>
    public bool CheckFilesExtensions(List<string> names)
    {
      if (names[0].EndsWith("dat"))
      {
        if (names[1].EndsWith("xml"))
          return true;
      }
      else
        if (names[0].EndsWith("xml"))
          if (names[1].EndsWith("dat"))
            return true;
      
      return false;
    }
    
    public static Stream CreateZip(Stream fileDat, Stream fileXml)
    {
      byte[] byteZip = new byte[0];
      try
      {
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
        
        byteZip = File.ReadAllBytes(fileZipName);
      }
      catch (Exception ex)
      {
        Logger.ErrorFormat("Isolated area. CreateZip. Error: {0}", ex.Message);
      }
      finally
      {
        Directory.Delete(path, true);
        return new MemoryStream(byteZip);
      }
    }
    
//    public static string CreateZip(string fileDat, string fileXml)
//    {
//      if (!Directory.Exists(path))
//      {
//        Directory.CreateDirectory(path);
//      }
//      
//      var zip = new ZipFile(fileZipName);
//      
//      CreateDefaultFile(fileDat, fileDatName);
//      CreateDefaultFile(fileXml, fileXmlName);
//      
//      zip.AddFile(fileDatName);
//      zip.AddFile(fileXmlName);
//      zip.Save();
//      
//      byte[] byteZip = File.ReadAllBytes(fileZipName);
//      
//      Directory.Delete(path, true);
//      
//      return Encoding.Default.GetString(byteZip);
//    }
    
    public static void CreateDefaultFile(Stream file, string name)
    {
      try
      {
        using (FileStream fs = File.Create(name))
        {
          file.CopyTo(fs);
        }
      }
      catch
      {
        throw AppliedCodeException.Create("Create default file error!");
      }
    }
    
//    public static void CreateDefaultFile(string file, string name)
//    {
//      try
//      {
//        using (FileStream fs = File.Create(name))
//        {
//          byte[] info = new UTF8Encoding(true).GetBytes(file);
//          fs.Write(info, 0, info.Length);
//        }
//      }
//      catch
//      {
//        throw AppliedCodeException.Create("Create default file error!");
//      }
//    }
  }
}