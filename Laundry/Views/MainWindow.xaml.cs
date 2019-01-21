using Laundry.Views;
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

namespace Laundry
{
  /// <summary>
  /// Логика взаимодействия для MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    public MainWindow()
    {
      InitializeComponent();
      /*
       
       */
    }

    private void OnLoginButtonClick(object sender, RoutedEventArgs e)
    {
      this.contentControl.Content = new DashBoardView();
    }

    private void ListBoxItem_OnSelected(object sender, RoutedEventArgs e)
    {
      this.contentControl.Content = new SettingsView();
    }

    private void OnDrawerEmployeeSelected(object sender, RoutedEventArgs e)
    {
      this.contentControl.Content = new EmployeeDictionaryView();
    }
  }
}


