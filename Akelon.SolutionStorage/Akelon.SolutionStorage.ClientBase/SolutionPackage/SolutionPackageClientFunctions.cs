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
    /// </summary>
    public void CreateFromZip()
    {
      var fileDialog = Dialogs.CreateInputDialog(Akelon.SolutionStorage.SolutionPackages.Resources.ZipInputDialogTitle);
      
      var file = fileDialog.AddFileSelect("Пакет", true);
      file.WithFilter(string.Empty, "zip");
      file.MaxFileSize(500 * 1024 * 1024);
      
      if (fileDialog.Show() == DialogButtons.Ok)
      {
        if (SolutionStorage.IsolatedFunctions.ZipHandler.CheckZipInput(Convert.ToBase64String(file.Value.Content)))
        {
          PublicFunctions.SolutionPackage.Remote.CreateFileFromZip(_obj);
        }
      }
    }
    
    /// <summary>
    /// Загрузка файлов с расширением .dat и .xml
    /// </summary>
    [Public, Remote]
    public void CreateFromDatXml()
    {
      var fileDialog = Dialogs.CreateInputDialog(Akelon.SolutionStorage.SolutionPackages.Resources.DatXmlInputDialogTitle);
      
      var files = fileDialog.AddFileSelectMany("Пакет", true);
      files.WithFilter(string.Empty, "dat", "xml");
      files.WithMaxFilesSize(500 * 1024 * 1024);
      
      // Придумать ебейший алгоритм проверки условий:
      // Количество файлов == 2
      // Один - '.dat', второй - 'xml'
      
//      foreach (var file in files.Value)
//      {
//        var extensions = file.FileName.
//      }
//      var extensions = files.Value.Where(f => f.FileName.Contains(".dat") || f.FileName.Contains(".xml"));
//      extensions = extensions.Distinct();
      
      
      if (fileDialog.Show() == DialogButtons.Ok)
      {
        if (SolutionStorage.IsolatedFunctions.ZipHandler.CheckZipInput(Convert.ToBase64String(file.Value.Content)))
        {
          PublicFunctions.SolutionPackage.Remote.CreateFileFromDatXml(_obj);
          package.Save();
        }
      }
    }
  }