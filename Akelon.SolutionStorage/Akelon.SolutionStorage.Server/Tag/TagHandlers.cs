using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;
using Akelon.SolutionStorage.Tag;

namespace Akelon.SolutionStorage
{
  partial class TagServerHandlers
  {

    public override void BeforeSave(Sungero.Domain.BeforeSaveEventArgs e)
    {
      //TODO:перенести все из бефо сейв  В новое действие "Выбрать тег" в справочнике решений
    }
  }
}