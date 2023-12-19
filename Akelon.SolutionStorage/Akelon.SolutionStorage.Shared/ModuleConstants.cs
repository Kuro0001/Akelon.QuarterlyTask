using System;
using Sungero.Core;

namespace Akelon.SolutionStorage.Constants
{
  public static class Module
  {
    
    /// <summary>
    /// Ключ параметра для выбора ОС системы, где установлена DRX
    /// </summary>
    public const string IsOsLinuxKey = "SolutionStorage_IsOsLinux";

    /// <summary>
    /// Ключ параметра для пути к папке с файлами, сохраняемыми изолированной областью ZipHandler
    /// </summary>
    public const string IsolatedDirectoryPathKey = "SolutionStorage_IsolatedDirectoryPath";

  }
}