using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;
using Akelon.SolutionStorage.SolutionPackage;
using System.IO;
using System.Text;

namespace Akelon.SolutionStorage.Server
{
  partial class SolutionPackageFunctions
  {

    /// <summary>
    /// 
    /// </summary>       
    public System.IO.MemoryStream GetMemoryStreamFromString(string input)
    {
      byte[] byteArray = Encoding.ASCII.GetBytes(input);
      return new MemoryStream(byteArray);
      
    }
    
    /// <summary>
    /// Создать экземпляр документа
    /// </summary>
    /// <returns>Новый экземпляр документа SolutionPackage</returns>
    [Public, Remote]
    public static Akelon.SolutionStorage.ISolutionPackage CreatePackage()
    {
      return SolutionPackages.Create();
    }
    
    /// <summary>
    /// Загрузка zip-архива с пакетом решения
    /// </summary>
    [Public, Remote]
    public void CreatePackageFromZip(string fileZip)
    {
      //      var package = SolutionPackages.Create();
      //      using (var memory = new System.IO.MemoryStream(fileContent))
      //      {
      //        package.CreateVersionFrom(memory, "zip");
      //        package.Save();
      //      }
    }

    /// <summary>
    /// Загрузка файлов с расширением .dat и .xml
    /// </summary>
    [Public, Remote]
    public void CreatePackageFromDatXml(string fileDat, string fileXml)
    {
      var fileString = Akelon.SolutionStorage.IsolatedFunctions.ZipHandler.CreateZipFromFiles();
      try
      {
        using (var memory = new System.IO.MemoryStream(Encoding.ASCII.GetBytes(fileString)))
        {
          _obj.CreateVersionFrom(memory, "zip");
          _obj.Save();
        }
      }
      catch
      {
        throw AppliedCodeException.Create("Server error: CreatePackageFromDatXml"); //TODO сделать ресурсом сообщение об ошибке
      }
    }
  }
}