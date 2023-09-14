using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;
using Akelon.SolutionStorage.PreparedSolution;

namespace Akelon.SolutionStorage.Server
{
  partial class PreparedSolutionFunctions
  {

    /// <summary>
    /// Создать новую запись справочника
    /// </summary>   
    [Remote]
    public static Akelon.SolutionStorage.IPreparedSolution CreatePreparedSolution()
    {
      return PreparedSolutions.Create();
    }

  }
}