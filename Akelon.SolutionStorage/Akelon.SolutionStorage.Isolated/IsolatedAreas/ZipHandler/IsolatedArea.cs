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
      
      try
      {
        if (!Directory.Exists(path))
        {
          Directory.CreateDirectory(path);
        }
        CreateDefaultFile(fileZip, fileZipName);
        var zip = new ZipFile(fileZipName);
        var fileNames = zip.EntryFileNames;
        Logger.DebugFormat("count = {0}, name1 = {1}", fileNames.Count, fileNames.FirstOrDefault());
        if (fileNames.Count == 2)
          return CheckFilesExtensions(fileNames.ToList());
        return false;
      }
      catch (Exception ex)
      {
        Logger.ErrorFormat("Isolated area. CheckZipInput. Error: {0}", ex.Message);
        return false;
      }
      finally
      {
        Directory.Delete(path, true);
      }
    }
    
    /// <summary>
    /// Проверить расширения файлов на наличие .dat и .xml файлов
    /// </summary>
    /// <param name="names">Список наименований файлов в количестве двух (2) штук.</param>
    /// <returns>Если в списке 2 файла и один из них .dat, второй - .xml, то результат = true, иначе - false.</returns>
    public static bool CheckFilesExtensions(List<string> names)
    {
      Logger.DebugFormat("CheckFilesExtensions");
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
    
    /// <summary>
    /// Создать zip-архив
    /// </summary>
    /// <param name="fileDat">файл формата dat</param>
    /// <param name="fileXml">файл формата xml</param>
    /// <returns>Zip-архив в виде потока</returns>
    public static Stream CreateZip(Stream fileDat, Stream fileXml)
    {
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
        
        return new MemoryStream(File.ReadAllBytes(fileZipName));
      }
      catch (Exception ex)
      {
        Logger.ErrorFormat("Isolated area. CreateZip. Error: {0}", ex.Message);
        return null;
      }
      finally
      {
        Directory.Delete(path, true);
      }
    }
    
    /// <summary>
    /// Создать временный файл
    /// </summary>
    /// <param name="file">Файл</param>
    /// <param name="name">Имя файла с указанием полного пути</param>
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
  }
}