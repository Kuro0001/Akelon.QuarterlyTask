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
    public void CreateFromZip()
    {
      var fileDialog = Dialogs.CreateInputDialog(Akelon.SolutionStorage.SolutionPackages.Resources.ZipInputDialogTitle);
      
      var fileSelect = fileDialog.AddFileSelect("zip-архив", true);
      fileSelect.WithFilter("Архивы с расширением .zip", "zip");
      fileSelect.MaxFileSize(500 * 1024 * 1024);
      
      if (fileDialog.Show() == DialogButtons.Ok)
      {
        var file = fileSelect.Value;
        if (!file.Name.Contains(".zip"))
        {
          throw AppliedCodeException.Create("Файл должен иметь расширение .zip");
        }
        CreateSimpleDocumentVersion(file);
        PublicFunctions.SolutionPackage.Remote.CreatePackageFromZip(_obj);
      }
    }
    
    /// <summary>
    /// Загрузить в версию документа файл с расширеним zip из файлов фармата .dat и .xml.
    /// </summary>
    /// <returns>Документ вида Пакет решения. Пакет в формате zip сохранен в версии документа.</returns>
    public void CreateFromDatXml()
    {
      var fileDialog = Dialogs.CreateInputDialog(Akelon.SolutionStorage.SolutionPackages.Resources.DatXmlInputDialogTitle);
      
      var fileSelect = fileDialog.AddFileSelectMany("Файлы", true);
      fileSelect.WithFilter($"Файлы с расширением .dat и .xml", "dat", "xml");
      fileSelect.WithMaxFilesSize(500 * 1024 * 1024);
      
      if (fileDialog.Show() == DialogButtons.Ok)
      {
        var files = fileSelect.Value;
        CreateSimpleDocuments(files);
        PublicFunctions.SolutionPackage.Remote.CreatePackageFromFiles(_obj); //TODO добавить простые доки без связей, не забыть удалить после работы с ними
      }
    }
    
    /// <summary>
    /// Задать документу наименование
    /// </summary>
    public void SetPackageName()
    {
      var fileNameDialog = Dialogs.CreateInputDialog("Введите наименование пакета");
      var fileName = fileNameDialog.AddString("Наименование: ", true);
      if (fileNameDialog.Show() == DialogButtons.Ok)
      {
        _obj.Name = fileName.Value;
      }
    }

    /// <summary>
    /// Создать временные простые документы для файлов .dat и .xml
    /// </summary>
    /// <param name="files"></param>
    public void CreateSimpleDocuments(System.Collections.Generic.IEnumerable<CommonLibrary.IFileObject> files)
    {
      var versions = new List<Sungero.Docflow.ISimpleDocument>(2);
      foreach (var file in files)
      {
        string extension = GetFileExtension(file);
        
        var simpleDoc = CreateSimpleDocumentVersion(file, extension);
        
        _obj.Relations.Add(Constants.SolutionPackage.PackageSourceBindType, simpleDoc);
        _obj.Relations.Save();
      }
    }
    
    
    public Sungero.Docflow.ISimpleDocument CreateSimpleDocumentVersion(CommonLibrary.IFileObject file, string extension)
    {
      var simpleDoc = Sungero.Docflow.SimpleDocuments.Create();
      simpleDoc.CreateVersionFrom(file.OpenReadStream(), extension);
      
      simpleDoc.Name = file.FileName;
      simpleDoc.Save();
      
      return simpleDoc;
    }
    
    public void CreateSimpleDocumentVersion(CommonLibrary.IBinaryObject file)
    {
      var simpleDoc = Sungero.Docflow.SimpleDocuments.Create();
      
      using (var memory = new MemoryStream(file.Content))
      {
        simpleDoc.CreateVersionFrom(memory, "zip");
      }
      
      simpleDoc.Name = file.Name;
      simpleDoc.Save();

      _obj.Relations.Add(Constants.SolutionPackage.PackageSourceBindType, simpleDoc);
      _obj.Relations.Save();
    }
    
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
        throw AppliedCodeException.Create("Файлы должны иметь расширение .dat и .xml!");
      }
    }
    
    public string GetStringFromFile(System.Collections.Generic.IEnumerable<CommonLibrary.IFileObject> files, string extension)
    {
      var file = GetDatXmlFile(files, extension);
      
      byte[] buffer = new Byte[file.OpenReadStream().Length + 10];
      var bytesCount = file.OpenReadStream().Read(buffer, 0, 500 * 1024 * 1024);
      var memory = new MemoryStream(buffer);
      var sr = new StreamReader(memory);
      
      return sr.ReadToEnd().ToString();
    }
    
    public CommonLibrary.IFileObject GetDatXmlFile(System.Collections.Generic.IEnumerable<CommonLibrary.IFileObject> files, string extension)
    {
      return files
        .Where(file => file.FileName.Contains(extension))
        .First();
    }
    
    public bool IsFilesContainsDatXml(System.Collections.Generic.IEnumerable<CommonLibrary.IFileObject> files)
    {
      if (files.Count() != 2)
      {
        Dialogs.NotifyMessage("Нужно выбрать два файла!");
        return false;
      }
      if (!files.Any(file => file.FileName.Contains("dat")))
      {
        Dialogs.NotifyMessage("Не найден файл с расширением dat");
        return false;
      }
      if (!files.Any(file => file.FileName.Contains(".xml")))
      {
        Dialogs.NotifyMessage("Не найден файл с расширением .xml");
        return false;
      }
      
      return true;
    }
  }
}