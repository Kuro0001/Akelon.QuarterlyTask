using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;
using Sungero.Domain.Initialization;

namespace Akelon.SolutionStorage.Server
{
  public partial class ModuleInitializer
  {
    public override void Initializing(Sungero.Domain.ModuleInitializingEventArgs e)
    {
      GrantRightsToSolutionStorage();
      CreatDocflowParams();
    }

    /// <summary>
    /// Выдать права на все элементы модуля SolutionStorage
    /// </summary>
    public void GrantRightsToSolutionStorage()
    {
      Akelon.SolutionStorage.PreparedSolutions.AccessRights.Grant(Roles.AllUsers, DefaultAccessRightsTypes.FullAccess);
      Akelon.SolutionStorage.PreparedSolutions.AccessRights.Save();
      Akelon.SolutionStorage.Tags.AccessRights.Grant(Roles.AllUsers, DefaultAccessRightsTypes.FullAccess);
      Akelon.SolutionStorage.Tags.AccessRights.Save();
      Akelon.SolutionStorage.SolutionPackages.AccessRights.Grant(Roles.AllUsers, DefaultAccessRightsTypes.FullAccess);
      Akelon.SolutionStorage.SolutionPackages.AccessRights.Save();
    }
    
    /// <summary>
    /// Создать параметры модуля в Docflow по дефолту
    /// </summary>
    public void CreatDocflowParams()
    {
      var isOsLinux = false;
      var isolatedDirectoryPath = @"C:\DirectumRX_SolutionStorage_TempDirectory";
      Sungero.Docflow.PublicFunctions.Module.InsertDocflowParam(Constants.Module.IsOsLinuxKey, isOsLinux.ToString());
      Sungero.Docflow.PublicFunctions.Module.InsertDocflowParam(Constants.Module.IsolatedDirectoryPathKey, isolatedDirectoryPath);
    }
  }
}
