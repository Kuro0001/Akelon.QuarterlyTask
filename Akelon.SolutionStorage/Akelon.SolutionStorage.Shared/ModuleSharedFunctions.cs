using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;
using System.IO;

namespace Akelon.SolutionStorage.Shared
{
  public class ModuleFunctions
  {

    /// <summary>
    /// Удалить устаревшие временные файлы изолированной области
    /// </summary>
    public void ClearTempIsolatedAreaDirectory()
    {
      var directory = new DirectoryInfo(SolutionStorage.IsolatedFunctions.ZipHandler.GetDirectoryPath());
      var delayDate = Calendar.Now.AddDays(-1);
      var files = directory.GetFiles()
        .Where(f => f.CreationTime <= delayDate);
      foreach(var file in files)
      {
        try
        {
          File.Delete(file.FullName);
        }
        catch (Exception ex)
        {
          Logger.ErrorFormat("ClearTempIsolatedAreaDirectory. File name = {0}. {1}", file.FullName, ex.Message);
        }
      }
    }

  }
}