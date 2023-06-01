using System;
using System.Collections.Generic;
using System.Linq;
using System.IO.Compression;
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
          Dialogs.ShowMessage("error on client in CreateFromZip");//TODO сделать ресурсом сообщение об ошибке
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
        if (!IsFilesContainsDatXml(files))
        {
          return;
        }
        var fileDat = GetDatXmlFile(files, ".dat");
        var fileXml = GetDatXmlFile(files, ".xml");
        PublicFunctions.SolutionPackage.Remote.CreatePackageFromDatXml(_obj, fileDat.ToString(), fileXml.ToString());
        if (string.IsNullOrEmpty(_obj.Name))
        {
          var fileNameDialog = Dialogs.CreateInputDialog("Введите наименование пакета");
          var fileName = fileNameDialog.AddString("Наименование: ",true);
          if (fileNameDialog.Show() == DialogButtons.Ok)
            _obj.Name = fileName.Value;
        }
      }
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
    
    public CommonLibrary.IFileObject GetDatXmlFile(System.Collections.Generic.IEnumerable<CommonLibrary.IFileObject> files, string extension)
    {
      return files
        .Where(file => file.FileName.Contains(extension))
        .First();
    }
  }
}