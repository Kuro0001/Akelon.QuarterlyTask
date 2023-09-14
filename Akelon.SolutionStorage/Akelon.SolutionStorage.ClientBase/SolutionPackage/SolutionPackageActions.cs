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

    public override void CreateFromFile(Sungero.Domain.Client.ExecuteActionArgs e)
    {
      //base.CreateFromFile(e);
      Functions.SolutionPackage.CreateFromZip(_obj);
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
    /// Загрузка файлов с расширением .dat и .xml
    /// </summary>
    public virtual void CreateFromDatXml(Sungero.Domain.Client.ExecuteActionArgs e)
    {
      Functions.SolutionPackage.CreateFromDatXml(_obj);
    }

    
    public virtual bool CanCreateFromZip(Sungero.Domain.Client.CanExecuteActionArgs e)
    {
      return true;
    }

    /// <summary>
    /// Создать эксземпляр документа и закрузить zip-файл, пакет решения, в версию
    /// </summary>
    public virtual void CreateFromZip(Sungero.Domain.Client.ExecuteActionArgs e)
    {
      var sp = Functions.SolutionPackage.Remote.CreatePackage();
      Functions.SolutionPackage.CreateFromZip(sp);
    }
  }
}