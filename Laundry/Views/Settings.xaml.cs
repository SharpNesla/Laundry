using System;
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
using Model;
using MaterialDesignThemes.Wpf;
using Laundry.Utils;
namespace Laundry.Views
{
  /// <summary>
  /// Interaction logic for Settings.xaml
  /// </summary>
  public class SettingsViewModel : DrawerActivityScreen
  {
    private readonly IShell _shell;

    public bool IsChecked
    {
      get { return _shell.IsDarkTheme; }
      set { _shell.IsDarkTheme = value; }
    }

    public SettingsViewModel(IEventAggregator aggregator, IModel model, IShell shell) : base(aggregator, model)
    {
      _shell = shell;
    }

    public void ChangeColorScheme(bool isDark)
    {
      this._shell.IsDarkTheme = isDark;
    }


  }
}