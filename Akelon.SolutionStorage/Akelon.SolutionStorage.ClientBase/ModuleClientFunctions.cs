using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;

namespace Akelon.SolutionStorage.Client
{
  public class ModuleFunctions
  {

    /// <summary>
    /// Создать пакет из dat, xml-файлов на обложке модуля SolutionStorage
    /// </summary>
    public virtual void CreatePackageFromDatXml()
    {
      var document = Akelon.SolutionStorage.PublicFunctions.SolutionPackage.Remote.CreatePackage();
      Functions.SolutionPackage.CreateFromDatXml(document);
    }
    
    /// <summary>
    /// Создать пакет из zip-файла на обложке модуля SolutionStorage
    /// </summary>
    public virtual void CreatePackageFromZip()
    {
      var document = Akelon.SolutionStorage.PublicFunctions.SolutionPackage.Remote.CreatePackage();
      Functions.SolutionPackage.CreateFromZip(document);
    }

    /// <summary>
    /// 
    /// </summary>
    public virtual void ShowIncompleteSolution()
    {
      var solutions = PreparedSolutions.GetAll(s => s.SolutionType == PreparedSolution.SolutionType.Incomplete);
      solutions.Show();
    }

    /// <summary>
    /// 
    /// </summary>
    public virtual void ShowCompleteSolution()
    {
      var solutions = PreparedSolutions.GetAll(s => s.SolutionType == PreparedSolution.SolutionType.Complete);
      solutions.Show();
    }
  }
}