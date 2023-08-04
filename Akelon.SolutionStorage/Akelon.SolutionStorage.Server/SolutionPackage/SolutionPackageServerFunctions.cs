using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;
using Akelon.SolutionStorage.SolutionPackage;
using System.IO;
using System.Text;

namespace Akelon.SolutionStorage.Server
{
  partial class SolutionPackageFunctions
  {

    /// <summary>
    /// 
    /// </summary>
    public System.IO.MemoryStream GetMemoryStreamFromString(string input)
    {
      byte[] byteArray = Encoding.ASCII.GetBytes(input);
      return new MemoryStream(byteArray);
      
    }
    
    /// <summary>
    /// Создать экземпляр документа
    /// </summary>
    /// <returns>Новый экземпляр документа SolutionPackage</returns>
    [Public, Remote]
    public static Akelon.SolutionStorage.ISolutionPackage CreatePackage()
    {
      return SolutionPackages.Create();
    }
    
    /// <summary>
    /// Загрузка zip-архива с пакетом решения
    /// </summary>
    [Public, Remote]
    public void CreatePackageFromZip(string fileZip)
    {
      //      var package = SolutionPackages.Create();
      //      using (var memory = new System.IO.MemoryStream(fileContent))
      //      {
      //        package.CreateVersionFrom(memory, "zip");
      //        package.Save();
      //      }
    }
    
    /// <summary>
    /// Загрузка файлов с расширением .dat и .xml
    /// </summary>
    //    [Public, Remote]
    //    public void CreatePackageFromDatXml(string fileDat, string fileXml)
    //    {
    //      var fileString = Akelon.SolutionStorage.IsolatedFunctions.ZipHandler.CreateZipFromFiles(fileDat, fileXml);
    //      var fileBytes = Encoding.ASCII.GetBytes(fileString);
    //      try
    //      {
    //        using (var sw = new MemoryStream(fileBytes))
    //        {
    //          _obj.CreateVersionFrom(sw, "zip");
    //        }
    //      }
    //      catch (Exception ex)
    //      {
    //        throw AppliedCodeException.Create($"Server error: CreatePackageFromDatXml. {ex.Message}"); //TODO сделать ресурсом сообщение об ошибке
    //      }
    //    }
    
    [Public, Remote]
    public void CreatePackageFromFiles()
    {
      var files = _obj.Relations.GetRelated();
      var fileDat = GetFileStream(files, "dat");
      var fileXml = GetFileStream(files, "xml");
      
      var file = Akelon.SolutionStorage.IsolatedFunctions.ZipHandler.CreateZipFromFiles(fileDat, fileXml);
      
      try
      {
        _obj.CreateVersionFrom(file, "zip");
      }
      catch (Exception ex)
      {
        throw AppliedCodeException.Create($"Server error: CreatePackageFromFiles. {ex.Message}"); //TODO сделать ресурсом сообщение об ошибке
      }
    }
    
    public System.IO.MemoryStream GetFileStream(System.Collections.Generic.IEnumerable<Sungero.Content.IElectronicDocument> files, string extension)
    {
      var file = GetDatXmlFile(files, extension);
      
      var fs = file.LastVersion.Body.Read();
      fs.Close();
      
      return fs;
    }
    
    public Sungero.Content.IElectronicDocument GetDatXmlFile(System.Collections.Generic.IEnumerable<Sungero.Content.IElectronicDocument> files, string extension)
    {
      return files
        .Where(file => file.Name.Contains(extension))
        .First();
    }
  }
}