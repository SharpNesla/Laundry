using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Laundry
{
  /// <summary>
  /// Логика взаимодействия для App.xaml
  /// </summary>
  public partial class App : Application
  {
    public static ShellViewModel CurrentWindow;
    public static Model.MockModel MockModel;
    internal new ShellViewModel ShellView;

    public App()
    {
      InitializeComponent();
    }
  }
}
