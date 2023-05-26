using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;
using Akelon.SolutionStorage.SolutionPackage;

namespace Akelon.SolutionStorage.Client
{
  internal static class SolutionPackageStaticActions
  {

  }

  partial class SolutionPackageActions
  {
    
    public virtual bool CanCreateFromZip(Sungero.Domain.Client.CanExecuteActionArgs e)
    {
      return true;
    }

    public override void CreateFromFile(Sungero.Domain.Client.ExecuteActionArgs e)
    {
      base.CreateFromFile(e);
    }

    public override bool CanCreateFromFile(Sungero.Domain.Client.CanExecuteActionArgs e)
    {
      return base.CanCreateFromFile(e);
    }

    public override void CreateVersionFromLastVersion(Sungero.Domain.Client.ExecuteActionArgs e)
    {
      base.CreateVersionFromLastVersion(e);
    }

    public override bool CanCreateVersionFromLastVersion(Sungero.Domain.Client.CanExecuteActionArgs e)
    {
      return base.CanCreateVersionFromLastVersion(e);
    }

    public virtual bool CanCreateFromDatXml(Sungero.Domain.Client.CanExecuteActionArgs e)
    {
      return true;
    }

    /// <summary>
    /// Загрузка zip-архива с пакетом решения
    /// </summary>
    public virtual void CreateFromZip(Sungero.Domain.Client.ExecuteActionArgs e)
    {
      var fileDialog = Dialogs.CreateInputDialog(Akelon.SolutionStorage.SolutionPackages.Resources.ZipInputDialogTitle);
      var file = GetFileFromDialog("zip");
      
      if (fileDialog.Show() == DialogButtons.Ok)
      {
        if (SolutionStorage.IsolatedFunctions.ZipHandler.CheckZipInput(Convert.ToBase64String(file.Value.Content)))
        {
          var fileContent = file.Value.Content;
          using (var memory = new System.IO.MemoryStream(fileContent))
          {
            _obj.CreateVersionFrom(memory, "zip");
          }
        }
      }
    }
    
    /// <summary>
    /// Загрузка файлов с расширением .dat и .xml
    /// </summary>
    public virtual void CreateFromDatXml(Sungero.Domain.Client.ExecuteActionArgs e)
    {
      var fileDialog = Dialogs.CreateInputDialog(Akelon.SolutionStorage.SolutionPackages.Resources.DatXmlInputDialogTitle);
      var file = GetFileFromDialog("dat", "xml");
      
      if (fileDialog.Show() == DialogButtons.Ok)
      {
        if (SolutionStorage.IsolatedFunctions.ZipHandler.CheckZipInput(Convert.ToBase64String(file.Value.Content)))
        {
          var fileContent = file.Value.Content;
          using (var memory = new System.IO.MemoryStream(fileContent))
          {
            _obj.CreateVersionFrom(memory, "zip");
          }
        }
      }
    }
    
    private CommonLibrary.IFileSelectDialogValue GetFileFromDialog(params string[] extension)
    {
      var file = fileDialog.AddFileSelect("Пакет", true);
      file.WithFilter(string.Empty, extension);
      file.MaxFileSize(500 * 1024 * 1024);
      
      return file;
    }
  }
}