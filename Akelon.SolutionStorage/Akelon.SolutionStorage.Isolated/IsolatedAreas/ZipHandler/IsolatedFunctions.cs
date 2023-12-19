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
    /// Создать zip-архив из файлов пакета решения
    /// </summary>
    /// <param name="fileDat">.dat файл</param>
    /// <param name="fileXml">.xml файл</param>
    /// <returns>zip-файл</returns>
    [Public]
    public Stream CreateZipFileFromDatXml(string directoryPath, bool isOsLinux, Stream fileDat, Stream fileXml)
    {
      var zipHelper = new ZipHelper(directoryPath, isOsLinux);
      return zipHelper.CreateZip(fileDat, fileXml);
    }

    /// <summary>
    /// Проверка наличия .dat и .xml файлов в zip-файле
    /// </summary>
    /// <param name="content">zip-файл</param>
    /// <returns>true - если в архиве два нужных файла, иначе - false</returns>
    [Public]
    public bool CheckZipInput(string directoryPath, bool isOsLinux, Stream fileZip)
    {
      var zipHelper = new ZipHelper(directoryPath, isOsLinux);
      return zipHelper.CheckZipInput(fileZip);
    }
  }
}