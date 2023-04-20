using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;
using Akelon.HelpDesk.InternalAddendumRequest;

namespace Akelon.HelpDesk
{
  partial class InternalAddendumRequestServerHandlers
  {

    public override void BeforeSave(Sungero.Domain.BeforeSaveEventArgs e)
    {
      base.BeforeSave(e);
      
      var employees = Akelon.StudySolution.Employees.GetAll();
      var dudes = employees.Where(emp => emp.IsDudeAkelon.HasValue && emp.IsDudeAkelon.Value);
      
      foreach (var dude in dudes)
      {
        _obj.AccessRights.Grant(dude, DefaultAccessRightsTypes.Read);
      }
    }
  }
}