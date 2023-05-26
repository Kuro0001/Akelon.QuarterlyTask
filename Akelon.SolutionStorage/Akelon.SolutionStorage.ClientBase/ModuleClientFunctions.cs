using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;

namespace Akelon.SolutionStorage.Client
{
  public class ModuleFunctions
  {

    /// <summary>
    /// 
    /// </summary>
    public virtual void Function1()
    {
      PublicFunctions.SolutionPackage.Remote.CreateFromZip();
    }

    /// <summary>
    /// 
    /// </summary>
    public virtual void ShowIncompleteSolution()
    {
      var solutions = PreparedSolutions.GetAll(s => s.SolutionType == PreparedSolution.SolutionType.Incomplete);
      solutions.Show();
    }

    /// <summary>
    /// 
    /// </summary>
    public virtual void ShowCompleteSolution()
    {
      var solutions = PreparedSolutions.GetAll(s => s.SolutionType == PreparedSolution.SolutionType.Complete);
      solutions.Show();
    }
  }
}