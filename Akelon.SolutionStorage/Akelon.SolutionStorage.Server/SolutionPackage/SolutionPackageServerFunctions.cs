using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;
using Akelon.SolutionStorage.SolutionPackage;

namespace Akelon.SolutionStorage.Server
{
  partial class SolutionPackageFunctions
  {
    /// <summary>
    /// Загрузка zip-архива с пакетом решения
    /// </summary>
    [Public, Remote]
    public void CreateFileFromZip()
    {
      var package = SolutionPackages.Create();
      using (var memory = new System.IO.MemoryStream(fileContent))
      {
        package.CreateVersionFrom(memory, "zip");
      }
    }

    /// <summary>
    /// Загрузка файлов с расширением .dat и .xml
    /// </summary>
    [Public, Remote]
    public void CreateFileFromDatXml()
    {
      var package = SolutionPackages.Create();
      using (var memory = new System.IO.MemoryStream(fileContent))
      {
        _obj.CreateVersionFrom(memory, "zip");
      }
    }
  }
}