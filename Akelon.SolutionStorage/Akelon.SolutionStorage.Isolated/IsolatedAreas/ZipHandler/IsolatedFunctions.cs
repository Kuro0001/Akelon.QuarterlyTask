using System;
using System.Collections.Generic;
using System.Linq;
using System.IO.Compression; //
using Newtonsoft.Json;
using Sungero.Core;
using Akelon.SolutionStorage.Structures.Module;
using Akelon.SolutionStorage.Isolated.ZipHandler;

namespace Akelon.SolutionStorage.Isolated.ZipHandler
{
  public class IsolatedFunctions
  {

    /// <summary>
    /// Создание zip-файл из файлов пакета решения
    /// </summary>
    /// <param name="fileDat">.dat файл</param>
    /// <param name="fileXml">.xml файл</param>
    /// <returns>zip-файл</returns>
    [Public]
    public System.IO.MemoryStream CreateZipFromFiles(System.IO.MemoryStream fileDat, System.IO.MemoryStream fileXml)
    {
      var result;
      
      
      return result;
    }

    /// <summary>
    /// Проверка наличия .dat и .xml файлов в zip-файле
    /// </summary>
    /// <param name="content">zip-файл</param>
    /// <returns>true - если в архиве два нужных файла, иначе - false</returns>
    [Public]
    public bool CheckZipInput(System.IO.MemoryStream content)
    {
      ZipHelper.Action();
      
      return false;
    }

  }
}