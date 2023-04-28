using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;
using Akelon.SolutionStorage.PreparedSolution;

namespace Akelon.SolutionStorage.Client
{
  partial class PreparedSolutionFunctions
  {
    /// <summary>
    /// Скрывает поля на форме в зависимости от выбранного типа решения
    /// </summary>
    /// <param name="solutionType">Введенный тип решения. При событии "Изменение контрола/свойства" необходимо ввести новое начение</param>
    public virtual void SetFieldsVisibilityWithSolutionType(Enumeration? solutionType)
    {
      if (solutionType.Equals(PreparedSolution.SolutionType.Complete))
      //if (solutionType.Any(s => s.Equals(PreparedSolution.SolutionType.Complete)))\
      //if (solutionType.Equals(PreparedSolution.SolutionType.Complete))
      {
        _obj.CodeExampleUrl = null;
        _obj.State.Properties.CodeExampleUrl.IsVisible = false;
        
        _obj.State.Properties.SolutionDocument.IsVisible = true;
        _obj.State.Properties.Package.IsVisible = true;
      }
      else if (solutionType.Equals(PreparedSolution.SolutionType.Incomplete))
      //else if (solutionType.Any(s => s.Equals(PreparedSolution.SolutionType.Incomplete)))
      //else if (solutionType.Equals(PreparedSolution.SolutionType.Incomplete))
      {
        _obj.State.Properties.CodeExampleUrl.IsVisible = true;
        
        _obj.SolutionDocument.Clear();
        _obj.Package.Clear();
        
        _obj.State.Properties.SolutionDocument.IsVisible = false;
        _obj.State.Properties.Package.IsVisible = false;
      }
    }
  }
}