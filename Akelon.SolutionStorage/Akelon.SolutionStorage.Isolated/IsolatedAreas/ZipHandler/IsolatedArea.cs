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
    /// Место расположения папки для временных файлов
    /// </summary>
    public string directoryPath = @"C:\DirectumRX_SolutionStorage_TempDirectory";
    /// <summary>
    /// Система расположена на ОС Линукс
    /// </summary>
    public bool isOsLinux = false;
    
    public ZipHelper(string directoryPath, bool isOsLinux)
    {
      Logger.DebugFormat("ZipHelper. Constructor. directoryPath = ({0}), isOsLinux = {1}", directoryPath, isOsLinux);
      this.isOsLinux = isOsLinux;
      this.directoryPath = directoryPath;
    }
    
    /// <summary>
    /// Удалить временный файл
    /// </summary>
    public static void DeleteTempFile(string name)
    {
      try
      {
        File.Delete(name);
        Logger.DebugFormat("ZipHelper. DeleteTempFile ({0}). Success", name);
      }
      catch (Exception ex)
      {
        Logger.ErrorFormat("ZipHelper. DeleteTempFile ({0}). Error: {1}", name, ex.Message);
      }
    }
    
    /// <summary>
    /// Получить уникальное имя для временного файла
    /// </summary>
    /// <param name="extension">расширение файла в формале ".ext"</param>
    /// <returns>Полное имя файла с расширением</returns>
    public string GetTempFileName(string extension)
    {
      var separator = isOsLinux ? '/' : '\\' ;
      var path = (directoryPath[directoryPath.Count() - 1] == separator) ? directoryPath : directoryPath + separator;
      var fullName = path + Guid.NewGuid().ToString() + extension;
       Logger.DebugFormat("ZipHelper. GetTempFileName. path = ({0}). extension = ({1})", path, extension);
      return fullName;
    }
    
    /// <summary>
    /// Проверить расширения файлов в zip-архиве на наличие .dat и .xml
    /// </summary>
    /// <param name="fileZip">Zip-файл, включающий в себя пакет решения</param>
    /// <returns>Если в архиве 2 файла и один из них .dat, второй - .xml, то результат = true, иначе - false.</returns>
    public bool CheckZipInput(Stream fileZip)
    {
      Logger.DebugFormat("ZipHelper. CheckZipInput");
      var tempFileName = string.Empty;
      try
      {
        // Проверить наличие папки для временных файлов
        if (!Directory.Exists(directoryPath))
        {
          Logger.DebugFormat("ZipHelper. CheckZipInput. Create directoryPath = ({0})", directoryPath);
          Directory.CreateDirectory(directoryPath);
        }
        // создать уникальное имя для временного файла
        tempFileName = GetTempFileName(".zip");
        
        CreateDefaultFile(fileZip, tempFileName);
        var zip = new ZipFile(tempFileName);
        // проверить содержание zip файла
        var fileNames = zip.EntryFileNames;
        // очистить переменную zip-файла, закрыть файл
        zip.Dispose();
        if (fileNames.Count == 2)
        {
          var isExtensionsCorrect = CheckFilesExtensions(fileNames.ToList());
          Logger.DebugFormat("ZipHelper. CheckZipInput. File name = ({0}). Return (isExtensionsCorrect) = {1}", tempFileName, isExtensionsCorrect);
          return isExtensionsCorrect;
        }
        Logger.DebugFormat("ZipHelper. CheckZipInput. File name = ({0}). Return false. Files count in zip < 2.", tempFileName);
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
        Logger.DebugFormat("ZipHelper. CheckFilesExtensions.");
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
    public Stream CreateZip(Stream fileDat, Stream fileXml)
    {
      var fileZipName = string.Empty;
      var fileDatName = string.Empty;
      var fileXmlName = string.Empty;
      try
      {
        // Проверить наличие папки для временных файлов
        if (!Directory.Exists(directoryPath))
        {
          Logger.DebugFormat("ZipHelper. CreateZip. Create directoryPath = ({0})", directoryPath);
          Directory.CreateDirectory(directoryPath);
        }
        // создать уникальные имена для временных файлов
        fileZipName = GetTempFileName(".zip");
        fileDatName = GetTempFileName(".dat");
        fileXmlName = GetTempFileName(".xml");        
        Logger.DebugFormat("ZipHelper. CreateZip. Zip name = ({0}), dat name = ({1}), xml name = ({2})", fileZipName, fileDatName, fileXmlName);
        
        var zip = new ZipFile(fileZipName);
        // создать временные файлы
        CreateDefaultFile(fileDat, fileDatName);
        CreateDefaultFile(fileXml, fileXmlName);
        // добавить файлы в архив
        zip.AddFile(fileDatName);
        zip.AddFile(fileXmlName);
        zip.Save();
        
         Logger.DebugFormat("ZipHelper. CreateZip. All done");
        return new MemoryStream(File.ReadAllBytes(fileZipName));
      }
      catch (Exception ex)
      {
        Logger.ErrorFormat("ZipHelper. CreateZip. Zip name = ({0}) Error: {1}", fileZipName, ex.Message);
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
          Logger.DebugFormat("ZipHelper. CreateDefaultFile. Name = ({0}). Success", name);
        }
      }
      catch (Exception ex)
      {
        Logger.ErrorFormat("ZipHelper. CreateDefaultFile, Name = {0}. Error: {1}", name, ex.Message);
        throw AppliedCodeException.Create(string.Format("ZipHelper. CreateDefaultFile, Name = {0}. Error: {1}", name, ex.Message));
      }
    }
  }
}