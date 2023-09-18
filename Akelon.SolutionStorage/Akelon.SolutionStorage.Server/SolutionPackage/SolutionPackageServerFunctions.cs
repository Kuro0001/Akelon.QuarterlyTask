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
    /// Создать новый документ
    /// </summary>
    /// <returns>Новый документ класса ElectronicDocument</returns>
    [Remote]
    public static Sungero.Docflow.ISimpleDocument CreateSimpleDocument()
    {
      return Sungero.Docflow.SimpleDocuments.Create();
    }
    
    /// <summary>
    /// Создать экземпляр документа
    /// </summary>
    /// <returns>Новый экземпляр документа SolutionPackage</returns>
    [Remote]
    public static Akelon.SolutionStorage.ISolutionPackage CreatePackage()
    {
      return SolutionPackages.Create();
    }
    
    /// <summary>
    /// Удалить связанные документы, использованные для создания версии пакета
    /// </summary>
    [Remote]
    public void DeleteRelatedPackagedDocuments()
    {
      var documents = _obj.Relations.GetRelated(Constants.SolutionPackage.PackageSourceBindType);
      foreach(var doc in documents)
      {
        Sungero.Content.ElectronicDocuments.Delete(doc);
      }
    }
    
    /// <summary>
    /// Загрузка zip-архива с пакетом решения
    /// </summary>
    [Remote]
    public void CreatePackageFromZip()
    {
      var files = _obj.Relations.GetRelated(Constants.SolutionPackage.PackageSourceBindType);
      var fileZip = GetFileWithExtension(files, "zip");
      
      using (var fileZipStream = fileZip.LastVersion.Body.Read())
      {
        bool isZipContainsFiles = Akelon.SolutionStorage.IsolatedFunctions.ZipHandler.CheckZipInput(fileZipStream);
        
        if (!isZipContainsFiles)
        {
          throw AppliedCodeException.Create("Архив не содержит файлов .dat и .xml!");
        }
        
        try
        {
          _obj.CreateVersionFrom(fileZipStream, "zip");
        }
        catch (Exception ex)
        {
          throw AppliedCodeException.Create(string.Format(Akelon.SolutionStorage.SolutionPackages.Resources.ErrorMessageTextCreatePackageFromZipFormat(ex.Message)));
        }
      }
    }
    
    [Remote]
    public void CreatePackageFromFiles()
    {
      var files = _obj.Relations.GetRelated(Constants.SolutionPackage.PackageSourceBindType);
      var fileDat = GetFileWithExtension(files, "dat");//TODO избавиться от функции, поменять на проверку по приложению обработчику
      var fileXml = GetFileWithExtension(files, "xml");
      
      using (var fileDatStream = fileDat.LastVersion.Body.Read())
      {
        using (var fileXmlStream = fileXml.LastVersion.Body.Read())
        {
          using (var stream = Akelon.SolutionStorage.IsolatedFunctions.ZipHandler.CreateZipFromFiles(fileDatStream, fileXmlStream))
          {
            try
            {
              _obj.CreateVersionFrom(stream, "zip");
            }
            catch (Exception ex)
            {
              throw AppliedCodeException.Create(string.Format(Akelon.SolutionStorage.SolutionPackages.Resources.ErrorMessageTextCreatePackageFromFilesFormat(ex.Message)));
            }
          }
        }
      }
    }
    
    public Sungero.Content.IElectronicDocument GetFileWithExtension(System.Collections.Generic.IEnumerable<Sungero.Content.IElectronicDocument> files, string extension)
    {
      return files
        .Where(file => file.Name.Contains(extension))
        .First();
    }
  }
}