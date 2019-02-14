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
    public partial class SubsidiaryDictionary: UserControl
  {
    public SubsidiaryDictionary()
    {
      InitializeComponent();

      MenuToggleButton.Checked +=
        (o, args) => (Application.Current as App).MainWindow.DrawerHost.IsLeftDrawerOpen = true;

      MenuToggleButton.Unchecked +=
        (o, args) => (Application.Current as App).MainWindow.DrawerHost.IsLeftDrawerOpen = false;
    }

    private void OnAddSubsidiaryButtonCLick(object sender, RoutedEventArgs e)
    {
      App.CurrentWindow.ChangeView(new SubsidiaryEditor(this));
    }
  }
}
