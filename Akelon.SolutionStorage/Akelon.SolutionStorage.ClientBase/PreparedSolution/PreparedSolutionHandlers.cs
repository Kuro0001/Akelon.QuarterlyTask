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

    public override void Showing(Sungero.Presentation.FormShowingEventArgs e)
    {
       // Присвоить начальное значение полю "Тип решения"
    }

    public virtual void SolutionTypeValueInput(Sungero.Presentation.EnumerationValueInputEventArgs e)
    {
      if (e.NewValue != null && e.NewValue != e.OldValue)
      {
        if (e.NewValue.Equals(PreparedSolution.SolutionType.Complete))
        {
          _obj.CodeExampleUrl = null;
          _obj.State.Properties.CodeExampleUrl.IsVisible = false;
          
          _obj.State.Properties.SolutionDocument.IsVisible = true;
          _obj.State.Properties.Package.IsVisible = true;
        }
        else if (e.NewValue.Equals(PreparedSolution.SolutionType.Incomplete))
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
}