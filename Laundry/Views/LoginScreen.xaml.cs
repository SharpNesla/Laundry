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
using MaterialDesignThemes.Wpf;

namespace Laundry.Views
{
  /// <summary>
  /// Interaction logic for LoginScreen.xaml
  /// </summary>
  public partial class LoginScreen : UserControl
  {
    public LoginScreen()
    {
      InitializeComponent();
    }

    private void OnLoginButtonClick(object sender, RoutedEventArgs e)
    {
      App.CurrentWindow.contentControl.Content = new DashBoard();
    }

    private void OnLoginSettingsButtonCLick(object sender, RoutedEventArgs e)
    {
      DialogHost.Show(new LoginSettings());
    }
  }
}
