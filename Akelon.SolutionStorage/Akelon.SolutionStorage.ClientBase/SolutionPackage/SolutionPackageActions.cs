using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;
using Akelon.SolutionStorage.SolutionPackage;

namespace Akelon.SolutionStorage.Client
{
  
  partial class SolutionPackageActions
  {
    
    public virtual bool CanCreateFromZip(Sungero.Domain.Client.CanExecuteActionArgs e)
    {
      return true;
    }

    public override void CreateFromFile(Sungero.Domain.Client.ExecuteActionArgs e)
    {
      base.CreateFromFile(e);
    }

    public override bool CanCreateFromFile(Sungero.Domain.Client.CanExecuteActionArgs e)
    {
      return base.CanCreateFromFile(e);
    }

    public override void CreateVersionFromLastVersion(Sungero.Domain.Client.ExecuteActionArgs e)
    {
      base.CreateVersionFromLastVersion(e);
    }

    public override bool CanCreateVersionFromLastVersion(Sungero.Domain.Client.CanExecuteActionArgs e)
    {
      return base.CanCreateVersionFromLastVersion(e);
    }

    public virtual bool CanCreateFromDatXml(Sungero.Domain.Client.CanExecuteActionArgs e)
    {
      return true;
    }

    /// <summary>
    /// Загрузка zip-архива с пакетом решения
    /// </summary>
    public virtual void CreateFromZip(Sungero.Domain.Client.ExecuteActionArgs e)
    {
      PublicFunctions.SolutionPackage.Remote.CreateFromZip(_obj);
    }
    
    /// <summary>
    /// Загрузка файлов с расширением .dat и .xml
    /// </summary>
    public virtual void CreateFromDatXml(Sungero.Domain.Client.ExecuteActionArgs e)
    {
      PublicFunctions.SolutionPackage.Remote.CreateFromDatXml(_obj);
    }
  }
}