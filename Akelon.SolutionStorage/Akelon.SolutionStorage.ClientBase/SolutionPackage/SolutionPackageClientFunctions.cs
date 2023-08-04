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
      var file = fileSelect.Value;
      
      if (fileDialog.Show() == DialogButtons.Ok)
      {
        if (string.IsNullOrEmpty(_obj.Name))
          _obj.Name = file.Name.Substring(0, file.Name.Length-4);
        if (SolutionStorage.IsolatedFunctions.ZipHandler.CheckZipInput(Convert.ToBase64String(file.Content)))
        {
          PublicFunctions.SolutionPackage.Remote.CreatePackageFromZip(_obj, Convert.ToBase64String(file.Content));
        }
        else
          Dialogs.ShowMessage("Client error in CreateFromZip"); //TODO сделать ресурсом сообщение об ошибке
      }
    }
    
    /// <summary>
    /// Загрузка файлов с расширением .dat и .xml
    /// </summary>
    public void CreateFromDatXml()
    {
      var fileDialog = Dialogs.CreateInputDialog(Akelon.SolutionStorage.SolutionPackages.Resources.DatXmlInputDialogTitle);
      
      var fileSelect = fileDialog.AddFileSelectMany("Файлы", true);
      fileSelect.WithFilter("Файлы с расширением .dat и .xml", "dat", "xml");
      fileSelect.WithMaxFilesSize(500 * 1024 * 1024);
      
      if (fileDialog.Show() == DialogButtons.Ok)
      {
        var files = fileSelect.Value;
        CreateSimpleDocumentVersions(files);
        
        if (string.IsNullOrEmpty(_obj.Name))
        {
          SetPackageName();
        }
      }
    }
    
    public void SetPackageName()
    {
      var fileNameDialog = Dialogs.CreateInputDialog("Введите наименование пакета");
      var fileName = fileNameDialog.AddString("Наименование: ", true);
      if (fileNameDialog.Show() == DialogButtons.Ok)
      {
        _obj.Name = fileName.Value;
        _obj.Save();
      }
    }
    
    public void CreateSimpleDocumentVersions(System.Collections.Generic.IEnumerable<CommonLibrary.IFileObject> files)
    {
      var versions = new List<Sungero.Docflow.ISimpleDocument>(2);
      foreach (var file in files)
      {
        byte[] buffer = new Byte[file.OpenReadStream().Length + 10];
        var bytesCount = file.OpenReadStream().Read(buffer, 0, 500 * 1024 * 1024);
        var memory = new MemoryStream(buffer);
        
        string extension = string.Empty;
        if (file.FileName.Contains("dat"))
        {
          extension = "dat";
        }
        else if (file.FileName.Contains("xml"))
        {
          extension = "xml";
        }
        var version = Sungero.Docflow.SimpleDocuments.Create();
        version.CreateVersionFrom(memory, extension);
        version.Name = file.FileName;
        
        _obj.Relations.Add("Addendum", version);
      }
      
      PublicFunctions.SolutionPackage.Remote.CreatePackageFromFiles(_obj);
    }
    
    //    public void CreatePackageFromDatXml(System.Collections.Generic.IEnumerable<CommonLibrary.IFileObject> files)
    //    {
    //      if (!IsFilesContainsDatXml(files))
    //      {
    //        return;
    //      }
//
    //      var fileDatContent = GetStringFromFile(files, ".dat");
    //      var fileXmlContent = GetStringFromFile(files, ".xml");
//
    //      PublicFunctions.SolutionPackage.Remote.CreatePackageFromDatXml(_obj, fileDatContent, fileXmlContent);
    //    }
    
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
      if (!files.Any(file => file.FileName.Contains(".dat")))
      {
        Dialogs.NotifyMessage("Не найден файл с расширением .dat");
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