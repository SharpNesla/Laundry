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


    }

    public void ChangeView(UserControl view)
    {
      this.contentControl.Content = view;
    }

    private void OnLoginButtonClick(object sender, RoutedEventArgs e)
    {
      this.contentControl.Content = new DashBoard();
    }

    private void ListBoxItem_OnSelected(object sender, RoutedEventArgs e)
    {
      this.contentControl.Content = new Settings();
    }

    private void OnDrawerEmployeeSelected(object sender, RoutedEventArgs e)
    {
      this.contentControl.Content = new EmployeeDictionary();
    }

    private void OnDrawerSubsidiarySelected(object sender, RoutedEventArgs e)
    {
      this.contentControl.Content = new SubsidiaryDictionary();
    }

    private void OnDrawerAboutSelected(object sender, RoutedEventArgs e)
    {
      this.contentControl.Content = new About();
    }

    private void OnDrawerDashboardSelected(object sender, RoutedEventArgs e)
    {
      this.contentControl.Content = new DashBoard();
    }

    private void OnDrawerCarDictionary(object sender, RoutedEventArgs e)
    {
      this.contentControl.Content = new CarDictionary();
    }

    private void OnDrawerDiscountSystemSelected(object sender, RoutedEventArgs e)
    {
      this.contentControl.Content = new DiscountSystem();
    }

    private void OnDrawerClientSelected(object sender, RoutedEventArgs e)
    {
      this.contentControl.Content = new ClientDictionary();
    }

    private void OnDrawerClothKindEditorSelected(object sender, RoutedEventArgs e)
    {
      this.ChangeView(new ClothKindEditor());
    }
  }
}


