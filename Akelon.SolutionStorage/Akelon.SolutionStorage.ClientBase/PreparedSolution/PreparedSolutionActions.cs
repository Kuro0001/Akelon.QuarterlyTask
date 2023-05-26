using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;
using Akelon.SolutionStorage.PreparedSolution;

namespace Akelon.SolutionStorage.Client
{
  internal static class PreparedSolutionPackageStaticActions
  {

    public static bool CanCreatePackage(Sungero.Domain.Client.CanExecuteChildCollectionActionArgs e)
    {
      return true;
    }

    public static void CreatePackage(Sungero.Domain.Client.ExecuteChildCollectionActionArgs e)
    {
      
    }
  }


}