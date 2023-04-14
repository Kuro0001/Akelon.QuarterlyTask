using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;
using Akelon.SolutionStorage.PreparedSolution;

namespace Akelon.SolutionStorage
{
  partial class PreparedSolutionServerHandlers
  {

    public override void BeforeSave(Sungero.Domain.BeforeSaveEventArgs e)
    {
      if (_obj.RealizationPlace.Any(rp => rp.CounterParty == null && string.IsNullOrEmpty(rp.Note)))
        e.AddError(Akelon.SolutionStorage.PreparedSolutions.Resources.EmptyRowRealizationPlace);
    }
  }

}