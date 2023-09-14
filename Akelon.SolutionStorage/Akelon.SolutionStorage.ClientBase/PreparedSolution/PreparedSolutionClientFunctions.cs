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
      if (solutionType.Equals(PreparedSolution.SolutionType.Package))
      {
        _obj.State.Properties.SolutionDocument.IsVisible = true;
        _obj.State.Properties.Package.IsVisible = true;
        _obj.State.Properties.DirectumRXVersion.IsRequired = true;
        
        _obj.WebsiteUrl = null;
        
        _obj.State.Properties.WebsiteUrl.IsVisible = false;
        _obj.State.Properties.WebsiteUrl.IsRequired = false;
      }
      else if (solutionType.Equals(PreparedSolution.SolutionType.Website))
      {
        _obj.State.Properties.WebsiteUrl.IsVisible = true;
        _obj.State.Properties.WebsiteUrl.IsRequired = true;
        
        _obj.SolutionDocument.Clear();
        _obj.Package.Clear();
        
        _obj.State.Properties.SolutionDocument.IsVisible = false;
        _obj.State.Properties.Package.IsVisible = false;
        _obj.State.Properties.DirectumRXVersion.IsRequired = false;
      }
    }
  }
}