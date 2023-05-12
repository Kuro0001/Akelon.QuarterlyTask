using System;
using System.Collections.Generic;
using System.Linq;
using System.IO.Compression;
using Sungero.Core;
using Sungero.CoreEntities;
using Akelon.SolutionStorage.SolutionPackage;

namespace Akelon.SolutionStorage.Client
{
  partial class SolutionPackageFunctions
  {

    /// <summary>
    /// 
    /// </summary>   
    public bool CheckZipInput(byte[] file)
    {
      if (Akelon.SolutionStorage.IsolatedFunctions.ZipHandler.CheckZipInput(file))
        return true;
      return false;
    }

  }
}