using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;
using System.IO;

namespace Akelon.SolutionStorage.Server
{
  public class ModuleFunctions
  {

    /// <summary>
    /// Удалить устаревшие временные файлы изолированной области
    /// </summary>
    public void ClearTempIsolatedAreaDirectory()
    {
      try
      {
        Logger.DebugFormat("SolutionStorage. ClearTempIsolatedAreaDirectory. Start");
        var directory = new DirectoryInfo(Sungero.Docflow.PublicFunctions.Module.GetDocflowParamsValue(Constants.Module.IsolatedDirectoryPathKey).ToString());
        var delayDate = Calendar.Now.AddDays(-1);
        var files = directory.GetFiles()
          .Where(f => f.CreationTime <= delayDate);
        Logger.DebugFormat("SolutionStorage. ClearTempIsolatedAreaDirectory. directory full name = ({0}), files count ({1}).", directory.FullName, files.Count());
        foreach(var file in files)
        {
          try
          {
            File.Delete(file.FullName);
             Logger.DebugFormat("SolutionStorage. ClearTempIsolatedAreaDirectory. File deleted. Full name = ({0}).", file.FullName);
          }
          catch (Exception ex)
          {
            Logger.ErrorFormat("SolutionStorage. ClearTempIsolatedAreaDirectory. File full name = {0}. Error: {1}", file.FullName, ex.Message);
            throw AppliedCodeException.Create(ex.Message);
          }
        }
      }
      catch (Exception ex)
      {
        Logger.ErrorFormat("SolutionStorage. ClearTempIsolatedAreaDirectory. Error: {0}", ex.Message);
        throw AppliedCodeException.Create(ex.Message);
      }
    }
    
  }
}