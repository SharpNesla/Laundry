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
using Laundry.Model;
using PropertyChanged;

namespace Laundry.Utils.Controls
{
  /// <summary>
  /// Interaction logic for OrderDataGrid.xaml
  /// </summary>
  
  public partial class ClothGrid : UserControl
  {


    public IObservableCollection<ClothInstance> ClothInstances
    {
      get { return (IObservableCollection<ClothInstance>)GetValue(ClothInstancesProperty); }
      set
      {

        this.DataContext = this;
        SetValue(ClothInstancesProperty, value);
      }
    }

    // Using a DependencyProperty as the backing store for ClothInstances.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ClothInstancesProperty =
        DependencyProperty.Register("ClothInstances", typeof(IObservableCollection<ClothInstance>), typeof(ClothGrid));



    public ClothGrid()
    {
      InitializeComponent();

    }
  }
}
