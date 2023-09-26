using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;
using Akelon.SolutionStorage.PreparedSolution;

namespace Akelon.SolutionStorage
{
  partial class PreparedSolutionClientHandlers
  {

    public override void Refresh(Sungero.Presentation.FormRefreshEventArgs e)
    {
      SolutionStorage.Functions.PreparedSolution.SetFieldsVisibilityWithSolutionType(_obj, _obj.SolutionKind);
    }

    public override void Showing(Sungero.Presentation.FormShowingEventArgs e)
    {
       SolutionStorage.Functions.PreparedSolution.SetFieldsVisibilityWithSolutionType(_obj, _obj.SolutionKind);
    }

    public virtual void SolutionKindValueInput(Sungero.Presentation.EnumerationValueInputEventArgs e)
    {
      if (e.NewValue != null && e.NewValue != e.OldValue)
      {
        SolutionStorage.Functions.PreparedSolution.SetFieldsVisibilityWithSolutionType(_obj, e.NewValue);
      }
    }
  }
}