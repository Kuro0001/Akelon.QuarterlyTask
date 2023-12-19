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
    /// Ввести значения параметров модуля
    /// </summary>
    public virtual void SetParametersAction()
    {
      if (!Users.Current.IncludedIn(Roles.Administrators))
      {
        Dialogs.ShowMessage(Akelon.SolutionStorage.Resources.ErrorTextCantSetParams,
                            Akelon.SolutionStorage.Resources.ErrorDescriptionAllowedOnlyToAdmins,
                            MessageType.Warning);
        return;
      }
      try
      {
        var dialog = Dialogs.CreateInputDialog(Akelon.SolutionStorage.Resources.DialogTitleSetModuleParams);
        // путь до папки куда распологаются временные файлы для из. области
        var isolatedDirectoryPathActual = Sungero.Docflow.PublicFunctions.Module.GetDocflowParamsValue(Constants.Module.IsolatedDirectoryPathKey).ToString();
        var isolatedDirectoryPath = dialog.AddString(Akelon.SolutionStorage.Resources.DialogParamTextModuleParamDirPath, true, isolatedDirectoryPathActual);
        // drx установлен на системе Линкус - true, иначе - false
        var isOsLinuxActual = Convert.ToBoolean(Sungero.Docflow.PublicFunctions.Module.GetDocflowParamsValue(Constants.Module.IsOsLinuxKey));
        var isOsLinux = dialog.AddBoolean(Akelon.SolutionStorage.Resources.DialogParamTextModuleIsOnLinux, isOsLinuxActual);
        
        if (dialog.Show() == DialogButtons.Ok)
        {
          Sungero.Docflow.PublicFunctions.Module.InsertOrUpdateDocflowParam(Constants.Module.IsolatedDirectoryPathKey, isolatedDirectoryPath.Value);
          Sungero.Docflow.PublicFunctions.Module.InsertOrUpdateDocflowParam(Constants.Module.IsOsLinuxKey, isOsLinux.Value.ToString());
          Functions.Module.ShowParametersAction();
        }
      }
      catch (Exception ex)
      {
        Dialogs.ShowMessage(Akelon.SolutionStorage.Resources.ErrorTextSetParams,
                            ex.Message,
                            MessageType.Error);
      }
    }

    /// <summary>
    /// Показать значения параметров модуля
    /// </summary>
    public virtual void ShowParametersAction()
    {
      try
      {
        var isolatedDirectoryPath = Sungero.Docflow.PublicFunctions.Module.GetDocflowParamsValue(Constants.Module.IsolatedDirectoryPathKey);
        var isOsLinux = Convert.ToBoolean(Sungero.Docflow.PublicFunctions.Module.GetDocflowParamsValue(Constants.Module.IsOsLinuxKey));
        var mesageDescription = String.Format("{0}: {1} \n {2}: {3}",
                                              Akelon.SolutionStorage.Resources.DialogParamTextModuleParamDirPath,
                                              isolatedDirectoryPath,
                                              Akelon.SolutionStorage.Resources.DialogParamTextModuleIsOnLinux,
                                              isOsLinux);
        Dialogs.ShowMessage(Akelon.SolutionStorage.Resources.DialogTextShowModuleParams,
                            mesageDescription,
                            MessageType.Information);
      }
      catch (Exception ex)
      {
        Dialogs.ShowMessage(Akelon.SolutionStorage.Resources.ErrorTextSetParams,
                            ex.Message,
                            MessageType.Error);
      }
    }

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
      try
      {
        if (Functions.SolutionPackage.CreateFromDatXml(document))
        {
          Functions.SolutionPackage.AskPackageName(document);
          document.Save();
          Dialogs.NotifyMessage(Akelon.SolutionStorage.Resources.MessageTextPackageCreatedSuccesfully);
        }
      }
      catch
      {
        SolutionPackages.Delete(document);
      }
    }
    
    /// <summary>
    /// Создать пакет из zip-файла на обложке модуля SolutionStorage
    /// </summary>
    public virtual void CreatePackageFromZip()
    {
      var document = Akelon.SolutionStorage.Functions.SolutionPackage.Remote.CreatePackage();
      try
      {
        if (Functions.SolutionPackage.CreateFromZip(document))
        {
          Functions.SolutionPackage.AskPackageName(document);
          document.Save();
          Dialogs.NotifyMessage(Akelon.SolutionStorage.Resources.MessageTextPackageCreatedSuccesfully);
        }
      }
      catch
      {
        SolutionPackages.Delete(document);
      }
    }

    /// <summary>
    /// Показать список готовых решений с типом - ссылка
    /// </summary>
    public virtual void ShowSolutionsWithThirdPartyResource()
    {
      PreparedSolutions.GetAll(s => s.SolutionKind == PreparedSolution.SolutionKind.ThirdPartyResource).Show();
    }

    /// <summary>
    /// Показать список готовых решений с типом - пакет
    /// </summary>
    public virtual void ShowSolutionWithPackages()
    {
      PreparedSolutions.GetAll(s => s.SolutionKind == PreparedSolution.SolutionKind.Package).Show();
    }
  }
}