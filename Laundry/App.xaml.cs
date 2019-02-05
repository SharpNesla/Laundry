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
    public static MainWindow CurrentWindow;
    public static Model.Model Model;
    internal new MainWindow MainWindow;
    protected override void OnStartup(StartupEventArgs e)
    {
      base.OnStartup(e);
      MainWindow window = new MainWindow();
      //TODO Remove all MainWindow uses
      MainWindow = window;
      CurrentWindow = window;
      Model = new Model.Model();
      window.Show();
    }
  }
}
