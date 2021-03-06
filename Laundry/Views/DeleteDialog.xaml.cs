﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Caliburn.Micro;
using Laundry.Utils;

namespace Laundry.Views
{
  /// <summary>
  /// Диалог удаления сущности в таблице
  /// </summary>
  public class DeleteDialogViewModel : Screen
  {
    public bool IsDelete { get; set; }

    /// <summary>
    /// Показать диалог удаления и вернуть соответствующее значение
    /// </summary>
    /// <returns></returns>
    public async Task<bool> AskQuestion()
    {
      await DialogHostExtensions.ShowCaliburnVM(this);
      var isDelete = this.IsDelete;
      IsDelete = false;
      return isDelete;
    }

    public void Remove()
    {
      IsDelete = true;
    }
  }
}