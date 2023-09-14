using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;
using Akelon.SolutionStorage.PreparedSolution;

namespace Akelon.SolutionStorage
{
  partial class PreparedSolutionFilteringServerHandler<T>
  {

    public override IQueryable<T> Filtering(IQueryable<T> query, Sungero.Domain.FilteringEventArgs e)
    {
      if (_filter != null && _filter.WebsiteFlag)
        query = query.Where(q => Equals(q.SolutionType, PreparedSolution.SolutionType.Website));
      if (_filter != null && _filter.PackageFlag)
        query = query.Where(q => Equals(q.SolutionType, PreparedSolution.SolutionType.Package));
      return query;
    }
  }

  partial class PreparedSolutionServerHandlers
  {

    public override void Created(Sungero.Domain.CreatedEventArgs e)
    {
      _obj.SolutionType = PreparedSolution.SolutionType.Website;
      if (Sungero.Company.Employees.GetAll().Where(em => em.Login == Users.Current.Login).Any())
        _obj.Responsible = Sungero.Company.Employees.As(Users.Current);
    }

    public override void BeforeSave(Sungero.Domain.BeforeSaveEventArgs e)
    {
      if (_obj.RealizationPlace.Any(rp => rp.Name == null && string.IsNullOrEmpty(rp.Note)))
        e.AddError(Akelon.SolutionStorage.PreparedSolutions.Resources.EmptyRowRealizationPlace);
    }
  }

}