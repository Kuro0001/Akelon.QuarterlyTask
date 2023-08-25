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
    /// Загрузка zip-архива с пакетом решения
    /// Если записи не присвоено наименование, то будет автоматически присваиваться наименование файла</summary>
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
        
        if (string.IsNullOrEmpty(_obj.Name))
        {
          SetPackageName();
        }
        
        CreateSimpleDocumentVersion(file);
        PublicFunctions.SolutionPackage.Remote.CreatePackageFromZip(_obj);
      }
      
      _obj.Save();
    }
    
    /// <summary>
    /// Загрузка файлов с расширением .dat и .xml
    /// </summary>
    public void CreateFromDatXml()
    {
      var fileDialog = Dialogs.CreateInputDialog(Akelon.SolutionStorage.SolutionPackages.Resources.DatXmlInputDialogTitle);
      
      var fileSelect = fileDialog.AddFileSelectMany("Файлы", true);
      fileSelect.WithFilter($"Файлы с расширением .dat и .xml", "dat", "xml");
      fileSelect.WithMaxFilesSize(500 * 1024 * 1024);
      
      if (fileDialog.Show() == DialogButtons.Ok)
      {
        // TODO: Убрать наименование при отмене окна
        SetPackageName();
        
        var files = fileSelect.Value;
        CreateSimpleDocumentVersions(files);
        PublicFunctions.SolutionPackage.Remote.CreatePackageFromFiles(_obj);
      }
      
      _obj.Save();
    }
    
    public void SetPackageName()
    {
      var fileNameDialog = Dialogs.CreateInputDialog("Введите наименование пакета");
      var fileName = fileNameDialog.AddString("Наименование: ", true);
      if (fileNameDialog.Show() == DialogButtons.Ok)
      {
        _obj.Name = fileName.Value;
      }
    }

    public void CreateSimpleDocumentVersions(System.Collections.Generic.IEnumerable<CommonLibrary.IFileObject> files)
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