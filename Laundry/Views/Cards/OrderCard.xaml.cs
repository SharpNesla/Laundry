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
using Laundry.Utils;
using Laundry.Utils.Controls;
using MaterialDesignThemes.Wpf;
using PropertyChanged;

namespace Laundry.Views
{
  public class OrderCardViewModel : Card<Order>
  {
    public ClothDataGridViewModel ClothInstancesGrid{ get; }

    public override Order Entity
    {
      get
      {
        return base.Entity;
      }

      set
      {
        base.Entity = value;
        this.ClothInstancesGrid.Order = value;
        this.ClothInstancesGrid.Refresh(0, 10);
      }
    }

    public OrderCardViewModel(IEventAggregator eventAggregator, ClothDataGridViewModel clothGrid) : base(eventAggregator, Screens.OrderEditor)
    {
      this.ClothInstancesGrid = clothGrid;
    }
  }
}
