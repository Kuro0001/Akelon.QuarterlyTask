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
    /// Создать тег
    /// </summary>
    public virtual void CreateTag()
    {
      SolutionStorage.Functions.Tag.Remote.CreateTag().Show();
    }

    /// <summary>
    /// Создать готовое решение
    /// </summary>
    public virtual void CreatePreparedSolution()
    {
      Akelon.SolutionStorage.Functions.PreparedSolution.Remote.CreatePreparedSolution().Show();
    }

    /// <summary>
    /// Создать пакет из dat, xml-файлов на обложке модуля SolutionStorage
    /// </summary>
    public virtual void CreatePackageFromDatXml()
    {
      var document = Akelon.SolutionStorage.Functions.SolutionPackage.Remote.CreatePackage();
      Functions.SolutionPackage.CreateFromDatXml(document);
      Functions.SolutionPackage.SetPackageName();
      document.Show();
    }
    
    /// <summary>
    /// Создать пакет из zip-файла на обложке модуля SolutionStorage
    /// </summary>
    public virtual void CreatePackageFromZip()
    {
      var document = Akelon.SolutionStorage.Functions.SolutionPackage.Remote.CreatePackage();
      Functions.SolutionPackage.CreateFromZip(document);
      Functions.SolutionPackage.SetPackageName();
      document.Show();
    }

    /// <summary>
    /// Показать список готовых решений с типом - ссылка
    /// </summary>
    public virtual void ShowSolutionsWithWebsites()
    {
      PreparedSolutions.GetAll(s => s.SolutionType == PreparedSolution.SolutionType.Website).Show();
    }

    /// <summary>
    /// Показать список готовых решений с типом - пакет
    /// </summary>
    public virtual void ShowSolutionWithPackages()
    {
      PreparedSolutions.GetAll(s => s.SolutionType == PreparedSolution.SolutionType.Package).Show();
    }
  }
}