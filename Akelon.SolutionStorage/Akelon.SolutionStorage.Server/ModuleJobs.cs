using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;

namespace Akelon.SolutionStorage.Server
{
  public class ModuleJobs
  {

    /// <summary>
    /// Удалить все связанные документы в пакетах решений
    /// </summary>
    public virtual void ClearSolutionPackageRelatedDocuments()
    {
      var documents = SolutionPackages.GetAll().Where(s => s.HasRelations);
      if (documents.Any())
      {
        foreach(var doc in documents)
          Functions.SolutionPackage.DeleteRelatedPackagedDocuments(doc);
      }
    }

  }
}