﻿using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;
using Akelon.SolutionStorage.PreparedSolution;

namespace Akelon.SolutionStorage.Client
{

  partial class PreparedSolutionActions
  {



    public virtual bool CanCreatePackageFromZip(Sungero.Domain.Client.CanExecuteActionArgs e)
    {
      return _obj.SolutionKind.Value.Equals(PreparedSolution.SolutionKind.Package);
    }

    public virtual void CreatePackageFromZip(Sungero.Domain.Client.ExecuteActionArgs e)
    {
      var package = _obj.PackageList.AddNew();
      try
      {
        package.Package = Functions.SolutionPackage.Remote.CreatePackage();
        Functions.SolutionPackage.CreateFromZip(package.Package);
        Functions.SolutionPackage.AskPackageName(package.Package);
        package.Package.Save();
      }
      catch (Exception ex)
      {
        _obj.PackageList.Remove(package);
        Dialogs.ShowMessage(ex.Message, MessageType.Error);
      }
    }

    public virtual bool CanCreatePackageFromDatXml(Sungero.Domain.Client.CanExecuteActionArgs e)
    {
      return _obj.SolutionKind.Value.Equals(PreparedSolution.SolutionKind.Package);
    }

    public virtual void CreatePackageFromDatXml(Sungero.Domain.Client.ExecuteActionArgs e)
    {
      var package = _obj.PackageList.AddNew();
      try
      {
        package.Package = Functions.SolutionPackage.Remote.CreatePackage();
        Functions.SolutionPackage.CreateFromDatXml(package.Package);
        Functions.SolutionPackage.AskPackageName(package.Package);
        package.Package.Save();
      }
      catch (Exception ex)
      {
        _obj.PackageList.Remove(package);
        Dialogs.ShowMessage(ex.Message, MessageType.Error);
      }
    }
  }
}