using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;
using Akelon.SolutionStorage.PreparedSolution;

namespace Akelon.SolutionStorage
{

  partial class PreparedSolutionRealizationPlaceSharedHandlers
  {

    public virtual void RealizationPlaceCounterpartyChanged(Akelon.SolutionStorage.Shared.PreparedSolutionRealizationPlaceCounterpartyChangedEventArgs e)
    {
      if (e.NewValue != null && e.NewValue != e.OldValue)
      {
        var line = _obj.Name = e.NewValue.Name;
      }
        
    }
  }

  partial class PreparedSolutionSharedHandlers
  {

  }
}