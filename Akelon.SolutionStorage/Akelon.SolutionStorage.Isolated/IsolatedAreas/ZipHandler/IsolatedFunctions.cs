using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Newtonsoft.Json;
using Sungero.Core;
using Akelon.SolutionStorage.Isolated.ZipHandler;

namespace Akelon.SolutionStorage.Isolated.ZipHandler
{
  public class IsolatedFunctions
  {

    /// <summary>
    /// Создать zip-архивиз файлов пакета решения
    /// </summary>
    /// <param name="fileDat">.dat файл</param>
    /// <param name="fileXml">.xml файл</param>
    /// <returns>zip-файл</returns>
    [Public]
    public Stream CreateFromDatXml(Stream fileDat, Stream fileXml)
    {
      return ZipHelper.CreateZip(fileDat, fileXml);
    }

    /// <summary>
    /// Проверка наличия .dat и .xml файлов в zip-файле
    /// </summary>
    /// <param name="content">zip-файл</param>
    /// <returns>true - если в архиве два нужных файла, иначе - false</returns>
    [Public]
    public bool CheckZipInput(Stream fileZip)
    {
      if (ZipHelper.CheckZipInput())
        return true;
      return false;
    }
  }
}