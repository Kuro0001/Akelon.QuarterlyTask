using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;
using Akelon.SolutionStorage.Tag;

namespace Akelon.SolutionStorage.Server
{
  partial class TagFunctions
  {

    /// <summary>
    /// Создать тег
    /// </summary>       
    [Remote]
    public static SolutionStorage.ITag CreateTag()
    {
      return Tags.Create();
    }

  }
}