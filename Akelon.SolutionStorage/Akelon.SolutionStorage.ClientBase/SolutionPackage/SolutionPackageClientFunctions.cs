using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Sungero.Core;
using Sungero.CoreEntities;
using Akelon.SolutionStorage.SolutionPackage;

namespace Akelon.SolutionStorage.Client
{
  partial class SolutionPackageFunctions
  {
    /// <summary>
    /// Загрузить в версию документа файл с расширеним zip.
    /// </summary>
    /// <returns>Документ вида Пакет решения. Пакет в формате zip сохранен в версии документа.</returns>
    public bool CreateFromZip()
    {
      if (string.IsNullOrEmpty(_obj.Name))
        _obj.Name = "temp_name";
      var fileDialog = Dialogs.CreateInputDialog(Akelon.SolutionStorage.SolutionPackages.Resources.ZipInputDialogTitle);
      
      var fileSelect = fileDialog.AddFileSelect(Akelon.SolutionStorage.SolutionPackages.Resources.ZipArchive, true);
      fileSelect.WithFilter(Akelon.SolutionStorage.SolutionPackages.Resources.ArchivesWithZipExtension, "zip");
      fileSelect.MaxFileSize(500 * 1024 * 1024);
      
      if (fileDialog.Show() == DialogButtons.Ok)
      {
        var file = fileSelect.Value;
        if (!file.Name.Contains(".zip"))
        {
          Dialogs.ShowMessage(Akelon.SolutionStorage.SolutionPackages.Resources.MessageErrorTextIncorrectFileExtension, MessageType.Error);
          return false;
        }
        using(var stream = new MemoryStream(file.Content))
        {
          CreateRelatedPackagedDocument(stream, file.Name, "zip");
        }
        
        Functions.SolutionPackage.Remote.CreatePackageFromZip(_obj);
        return true;
      }
      return false;
    }
    
    /// <summary>
    /// Загрузить в версию документа файл с расширеним zip из файлов фармата .dat и .xml.
    /// </summary>
    /// <returns>Документ вида Пакет решения. Пакет в формате zip сохранен в версии документа.</returns>
    public bool CreateFromDatXml()
    {
      if (string.IsNullOrEmpty(_obj.Name))
        _obj.Name = "temp_name";
      var fileDialog = Dialogs.CreateInputDialog(Akelon.SolutionStorage.SolutionPackages.Resources.DatXmlInputDialogTitle);
      
      var fileSelect = fileDialog.AddFileSelectMany(Akelon.SolutionStorage.SolutionPackages.Resources.Files, true);
      fileSelect.WithFilter(Akelon.SolutionStorage.SolutionPackages.Resources.FilesWithDatAndXmlExtensions, "dat", "xml");
      fileSelect.WithMaxFilesSize(500 * 1024 * 1024);
      
      if (fileDialog.Show() == DialogButtons.Ok)
      {
        var files = fileSelect.Value;
        foreach(var file in files)
        {
          CreateRelatedPackagedDocument(file.OpenReadStream(), file.FileName, GetFileExtension(file));
        }
        Functions.SolutionPackage.Remote.CreatePackageFromDatXml(_obj);
        return true;
      }
      return false;
    }
    
    /// <summary>
    /// Задать документу наименование
    /// </summary>
    public void AskPackageName()
    {
      var fileNameDialog = Dialogs.CreateInputDialog(Akelon.SolutionStorage.SolutionPackages.Resources.EnterPackageName);
      var fileName = fileNameDialog.AddString(Akelon.SolutionStorage.SolutionPackages.Resources.Name, true);
      if (fileNameDialog.Show() == DialogButtons.Ok)
      {
        _obj.Name = fileName.Value;
      }
    }
    
    
    /// <summary>
    /// Создать временный простый документ и создать связь файла с Готовым решением
    /// </summary>
    /// <param name="stream">Контент файла, который необходимо сохранить в новый экземпляр документа и создать связь с актуальным экземпляром Готового решения.</param>
    /// <param name="name">Наименование файла.</param>
    /// <param name="extension">Расширение файла.</param>
    public void CreateRelatedPackagedDocument(System.IO.Stream stream, string name, string extension)
    {
      var simpleDoc = Functions.SolutionPackage.Remote.CreateSimpleDocument();
      simpleDoc.CreateVersionFrom(stream, extension);
      simpleDoc.Name = name;
      simpleDoc.Save();
      
      _obj.Relations.Add(Constants.SolutionPackage.PackageSourceBindType, simpleDoc);
      _obj.Relations.Save();
    }
    
    /// <summary>
    /// Получить расширение файла
    /// </summary>
    /// <param name="file">Файл, получаемый из диалогового окна</param>
    /// <returns>Расширение файла в формате строки со значением - "dat" или "xml"</returns>
    public string GetFileExtension(CommonLibrary.IFileObject file)
    {
      string fileName = file.FileName;
      
      if (fileName.Contains($".dat"))
      {
        return "dat";
      }
      else if (fileName.Contains($".xml"))
      {
        return "xml";
      }
      else
      {
        Logger.ErrorFormat("GetFileExtension. {0}", Akelon.SolutionStorage.SolutionPackages.Resources.ErrorMessageTextThereAreNoDatOrXml);
        throw AppliedCodeException.Create(Akelon.SolutionStorage.SolutionPackages.Resources.ErrorMessageTextThereAreNoDatOrXml);
      }
    }
  }
}