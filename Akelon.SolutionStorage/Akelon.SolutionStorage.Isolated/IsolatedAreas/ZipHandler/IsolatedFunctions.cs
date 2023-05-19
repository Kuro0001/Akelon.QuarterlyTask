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
    public string CreateZipFromFiles(string fileDat, string fileXml)
    {
      var result = string.Empty;
      
      
      return result;
    }

    /// <summary>
    /// Проверка наличия .dat и .xml файлов в zip-файле
    /// </summary>
    /// <param name="content">zip-файл</param>
    /// <returns>true - если в архиве два нужных файла, иначе - false</returns>
    [Public]
    public bool CheckZipInput(string content)
    {
      if (ZipHelper.Action())
        return true;
      return false;
    }

  }
}