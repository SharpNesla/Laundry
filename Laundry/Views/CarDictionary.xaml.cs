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

namespace Laundry.Views
{
  /// <summary>
  /// Interaction logic for CarDictionary.xaml
  /// </summary>
  public partial class CarDictionary : UserControl
  {
    public CarDictionary()
    {
      InitializeComponent();

      MenuToggleButton.Checked +=
        (o, args) => (Application.Current as App).MainWindow.DrawerHost.IsLeftDrawerOpen = true;

      MenuToggleButton.Unchecked +=
        (o, args) => (Application.Current as App).MainWindow.DrawerHost.IsLeftDrawerOpen = false;
    }
  }
}