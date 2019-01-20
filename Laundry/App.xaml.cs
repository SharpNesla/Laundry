using System;
using System.Collections.Generic;
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
    internal new MainWindow MainWindow;
    protected override void OnStartup(StartupEventArgs e)
    {
      base.OnStartup(e);
      MainWindow window = new MainWindow();
      this.MainWindow = window;
      window.Show();
    }
  }
}
