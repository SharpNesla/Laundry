using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Laundry.Utils;
using MaterialDesignThemes.Wpf;

namespace Laundry.Views
{
  /// <summary>
  /// Interaction logic for OrderEditor.xaml
  /// </summary>
  public partial class OrderEditorViewModel : ActivityScreen
  {
    public DateTime DateTime { get; set; }



    public BindableCollection<ClothInstance> ClothInstances { get; set; }

    public OrderEditorViewModel(IEventAggregator aggregator, IModel model, ClothEditor editor) : base(aggregator, model)
    {
      var kind = new ClothKind { MeasureKind = MeasureKind.Kg, Name = "Носки" };

      this.ClothInstances = new BindableCollection<ClothInstance>(
        new[]
        {
          new ClothInstance {Amount = 3, Kind = kind, WearPercentage = 0},
          new ClothInstance {Amount = 3, Kind = kind, WearPercentage = 0},
          new ClothInstance {Amount = 3, Kind = kind, WearPercentage = 0},
          new ClothInstance {Amount = 3, Kind = kind, WearPercentage = 0},
          new ClothInstance {Amount = 3, Kind = kind, WearPercentage = 0},
        }
      );
    }

    public void Discard()
    {
      ChangeApplicationScreen(Screens.DashBoard);
    }

    private void OnApplyButtonClick(object sender, RoutedEventArgs e)
    {
    }

    public void AddOrder()
    {

    }

    private void OnAddClothButtonClick(object sender, RoutedEventArgs e)
    {
      //DialogHost.Show();
    }

    
  }
}