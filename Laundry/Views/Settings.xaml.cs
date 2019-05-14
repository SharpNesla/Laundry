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
    private readonly PaletteHelper _paletteHelper;
    private bool _isChecked;

    public bool IsChecked
    {
      get { return _isChecked; }
      set
      {
        _isChecked = value; 

      }
    }

    public SettingsViewModel(IEventAggregator aggregator, IModel model) : base(aggregator, model)
    {
      this._paletteHelper = new PaletteHelper();
      this._isChecked = true;
    }

    public void ChangeColorScheme(bool isDark)
    {
      _paletteHelper.SetLightDark(isDark);
    }


  }
}