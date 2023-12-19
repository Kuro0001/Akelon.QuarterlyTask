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
      var fileZip = _obj.Relations.GetRelated(Constants.SolutionPackage.PackageSourceBindType).Where(f => string.Equals(f.LastVersion.AssociatedApplication.Extension, "zip")).FirstOrDefault();
      if (fileZip == null)
      {
        Logger.ErrorFormat("CreatePackageFromZip. file id = {0}. {1}", _obj.Id, Akelon.SolutionStorage.SolutionPackages.Resources.ErrorMessageTextPackegDoesntHaveXmlFile);
        throw AppliedCodeException.Create(Akelon.SolutionStorage.SolutionPackages.Resources.ErrorMessageTextPackegDoesntHaveXmlFile);
      }
      
      using (var fileZipStream = fileZip.LastVersion.Body.Read())
      {
        var isolatedDirectoryPath = Sungero.Docflow.PublicFunctions.Module.GetDocflowParamsValue(Constants.Module.IsolatedDirectoryPathKey).ToString();
        var isOsLinux = Convert.ToBoolean(Sungero.Docflow.PublicFunctions.Module.GetDocflowParamsValue(Constants.Module.IsOsLinuxKey));
        bool isZipContainsFiles = Akelon.SolutionStorage.IsolatedFunctions.ZipHandler.CheckZipInput(isolatedDirectoryPath, isOsLinux, fileZipStream);
        
        if (!isZipContainsFiles)
        {
          Logger.ErrorFormat("CreatePackageFromZip. file id = {0}. {1}", _obj.Id, Akelon.SolutionStorage.SolutionPackages.Resources.ErrorMessageTextIncorrectZipContent);
          throw AppliedCodeException.Create(Akelon.SolutionStorage.SolutionPackages.Resources.ErrorMessageTextIncorrectZipContent);
        }
        
        try
        {
          _obj.CreateVersionFrom(fileZipStream, "zip");
        }
        catch (Exception ex)
        {
          Logger.ErrorFormat("CreatePackageFromZip. file id = {0}. {1}", _obj.Id, Akelon.SolutionStorage.SolutionPackages.Resources.ErrorMessageTextCreatePackageFromZipFormat(ex.Message));
          throw AppliedCodeException.Create(string.Format(Akelon.SolutionStorage.SolutionPackages.Resources.ErrorMessageTextCreatePackageFromZipFormat(ex.Message)));
        }
      }
    }
    
    /// <summary>
    /// Сохранить пакет решения, представленный двумя файлами .dat и .xml, в новую версию документа в формате zip-файла
    /// </summary>
    [Remote]
    public void CreatePackageFromDatXml()
    {
      var files = _obj.Relations.GetRelated(Constants.SolutionPackage.PackageSourceBindType);
      var fileDat = files.Where(f => f.LastVersion.AssociatedApplication.Extension == "dat").FirstOrDefault();
      var fileXml = files.Where(f => f.LastVersion.AssociatedApplication.Extension == "xml").FirstOrDefault();
      
      if (fileDat == null || fileXml == null)
      {
        Logger.ErrorFormat("CreatePackageFromDatXml. file id = {0}. {1}", _obj.Id, Akelon.SolutionStorage.SolutionPackages.Resources.ErrorMessageTextInRelatedFilesNoDatOrXmlFiles);
        throw AppliedCodeException.Create(Akelon.SolutionStorage.SolutionPackages.Resources.ErrorMessageTextInRelatedFilesNoDatOrXmlFiles);
      }
      using (var fileDatStream = fileDat.LastVersion.Body.Read())
      {
        using (var fileXmlStream = fileXml.LastVersion.Body.Read())
        {
          var isolatedDirectoryPath = Sungero.Docflow.PublicFunctions.Module.GetDocflowParamsValue(Constants.Module.IsolatedDirectoryPathKey).ToString();
          var isOsLinux = Convert.ToBoolean(Sungero.Docflow.PublicFunctions.Module.GetDocflowParamsValue(Constants.Module.IsOsLinuxKey));
          using (var stream = Akelon.SolutionStorage.IsolatedFunctions.ZipHandler.CreateZipFileFromDatXml(isolatedDirectoryPath, isOsLinux, fileDatStream, fileXmlStream))
          {
            try
            {
              _obj.CreateVersionFrom(stream, "zip");
            }
            catch (Exception ex)
            {
              Logger.ErrorFormat("CreatePackageFromDatXml. file id = {0}. {1}", _obj.Id, Akelon.SolutionStorage.SolutionPackages.Resources.ErrorMessageTextCreatePackageFromFilesFormat(ex.Message));
              throw AppliedCodeException.Create(Akelon.SolutionStorage.SolutionPackages.Resources.ErrorMessageTextCreatePackageFromFilesFormat(ex.Message));
            }
          }
        }
      }
    }
  }
}