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
  /// Interaction logic for ClothEditor.xaml
  /// </summary>
  public partial class ClothEditor : UserControl
  {
    private UserControl _context;

    public ClothEditor(UserControl context)
    {
      InitializeComponent();
      this._context = context;
    }

    private void OnDisableButtonClick(object sender, RoutedEventArgs e)
    {
      App.CurrentWindow.ChangeView(_context);
    }

    private void OnApplyButtonClick(object sender, RoutedEventArgs e)
    {
    }
  }
}