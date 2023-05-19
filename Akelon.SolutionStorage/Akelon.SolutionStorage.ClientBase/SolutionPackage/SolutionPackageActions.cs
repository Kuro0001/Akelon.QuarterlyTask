using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;
using Akelon.SolutionStorage.SolutionPackage;

namespace Akelon.SolutionStorage.Client
{
  partial class SolutionPackageActions
  {
    /// <summary>
    /// Загружаем готовый zip файл
    /// </summary>
    public override void CreateFromFile(Sungero.Domain.Client.ExecuteActionArgs e)
    {
      //base.CreateFromFile(e);
      var fileDialog = Dialogs.CreateInputDialog("Выберите пакет");
      var file = fileDialog.AddFileSelect("zip файл", true);
      file.WithFilter(string.Empty, "zip");
      file.MaxFileSize(500*1024*1024);
      if (fileDialog.Show() == DialogButtons.Ok)
      {
        if (SolutionStorage.Functions.SolutionPackage.CheckZipInput(_obj, Convert.ToBase64String(file.Value.Content)))
        {
          var fileContent = file.Value.Content;
          using (var memory = new System.IO.MemoryStream(fileContent))
          {
            _obj.CreateVersionFrom(memory, "zip");
          }
        }
      }
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

  }

}