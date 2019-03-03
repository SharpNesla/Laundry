using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using MaterialDesignThemes.Wpf;

namespace Laundry.Views
{
  /// <summary>
  /// Interaction logic for DashBoard.xaml
  /// </summary>
  public partial class EmployeeEditor : UserControl
  {

        private UserControl _context;

        public EmployeeEditor(UserControl context)
        {
            InitializeComponent();
            this._context = context;
        }

        private void OnDisableButtonClick(object sender, RoutedEventArgs e)
        {
            //App.CurrentWindow.ChangeView(_context);
        }

        private void OnApplyButtonClick(object sender, RoutedEventArgs e)
        {

        }
    }
}