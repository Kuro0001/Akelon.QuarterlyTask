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
    /// <summary>
    /// Место расположения папки для временных файлов в формате ( @"C:\путь\" )
    /// </summary>
    const string directoryPath = @"C:\DirectumRX_SolutionStorage_TempDirectory\";
    
    /// <summary>
    /// Получить место расположения папки для временных файлов
    /// </summary>
    /// <returns>формат ( @"C:\путь\" )</returns>
    public static string GetDirectoryPath()
    {
      return directoryPath;
    }
    
    public ZipHelper() { }
    
    /// <summary>
    /// Удалить временный файл
    /// </summary>
    public static void DeleteTempFile(string name)
    {
      try
      {
        File.Delete(name);
      }
      catch (Exception ex)
      {
        Logger.ErrorFormat("ZipHelper. DeleteTempFile ({0}). Error: {1}", name, ex.Message);
      }
    }
    
    /// <summary>
    /// Получить уникальное имя для временного файла
    /// </summary>
    /// <param name="path">путь до места, где должен находиться файл</param>
    /// <param name="extension">расширение файла в формале ".ext"</param>
    /// <returns>Полное имя файла с расширением</returns>
    public static string GetTempFileName(string path, string extension)
    {
      var fullName = path + Guid.NewGuid().ToString() + extension;
      return fullName;
//      if (File.Exists(fullName))
//      {
//        // Рекурсивный вызов функции до момента, пока не будет найдено уникальное имя для временного файла
//        retun this.GetTempFileName(path, extension);
//      }
//      else
//        return fullName;
    }
    
    /// <summary>
    /// Проверить расширения файлов в zip-архиве на наличие .dat и .xml
    /// </summary>
    /// <param name="fileZip">Zip-файл, включающий в себя пакет решения</param>
    /// <returns>Если в архиве 2 файла и один из них .dat, второй - .xml, то результат = true, иначе - false.</returns>
    public static bool CheckZipInput(Stream fileZip)
    {
      var tempFileName = string.Empty;
      try
      {
        // Проверить наличие папки для временных файлов
        if (!Directory.Exists(directoryPath))
        {
          Directory.CreateDirectory(directoryPath);
        }
        // создать уникальное имя для временного файла
        tempFileName = GetTempFileName(directoryPath, ".zip");
        
        CreateDefaultFile(fileZip, tempFileName);
        var zip = new ZipFile(tempFileName);
        //проверить содержание zip файла
        var fileNames = zip.EntryFileNames;
        if (fileNames.Count == 2)
          return CheckFilesExtensions(fileNames.ToList());
        return false;
      }
      catch (Exception ex)
      {
        Logger.ErrorFormat("ZipHelper. CheckZipInput. File name = {0} . Error: {1}", tempFileName, ex.Message);
        return false;
      }
      finally
      {
        DeleteTempFile(tempFileName);
      }
    }
    
    /// <summary>
    /// Проверить расширения файлов на наличие .dat и .xml файлов
    /// </summary>
    /// <param name="names">Список наименований файлов в количестве двух (2) штук.</param>
    /// <returns>Если в списке 2 файла и один из них .dat, второй - .xml, то результат = true, иначе - false.</returns>
    public static bool CheckFilesExtensions(List<string> names)
    {
      try
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
      catch (Exception ex)
      {
        Logger.ErrorFormat("ZipHelper. CheckFilesExtensions. Error: {0}", ex.Message);
        return false;
      }
    }
    
    /// <summary>
    /// Создать zip-архив
    /// </summary>
    /// <param name="fileDat">файл формата dat</param>
    /// <param name="fileXml">файл формата xml</param>
    /// <returns>Zip-архив в виде потока</returns>
    public static Stream CreateZip(Stream fileDat, Stream fileXml)
    {
      var fileZipName = string.Empty;
      var fileDatName = string.Empty;
      var fileXmlName = string.Empty;
      try
      {
        // Проверить наличие папки для временных файлов
        if (!Directory.Exists(directoryPath))
        {
          Directory.CreateDirectory(directoryPath);
        }
        // создать уникальные имена для временных файлов
        fileZipName = GetTempFileName(directoryPath, ".zip");
        fileDatName = GetTempFileName(directoryPath, ".dat");
        fileXmlName = GetTempFileName(directoryPath, ".xml");
        
        var zip = new ZipFile(fileZipName);
        // создать временные файлы
        CreateDefaultFile(fileDat, fileDatName);
        CreateDefaultFile(fileXml, fileXmlName);
        // добавить файлы в архив
        zip.AddFile(fileDatName);
        zip.AddFile(fileXmlName);
        zip.Save();
        
        return new MemoryStream(File.ReadAllBytes(fileZipName));
      }
      catch (Exception ex)
      {
        Logger.ErrorFormat("ZipHelper. CreateZip. Error: {0}", ex.Message);
        return null;
      }
      finally
      {
        DeleteTempFile(fileZipName);
        DeleteTempFile(fileDatName);
        DeleteTempFile(fileXmlName);
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
        Logger.ErrorFormat("ZipHelper. CreateDefaultFile, Name = {0}. Error: Create default file error", name);
        throw AppliedCodeException.Create(string.Format("ZipHelper. CreateDefaultFile, Name = {0}. Error: Create default file error", name));
      }
    }
  }
}