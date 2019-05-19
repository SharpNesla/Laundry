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
using Model;
using Laundry.Utils;
using Laundry.Utils.Controls;
using MaterialDesignThemes.Wpf;
using MongoDB.Driver;
using PropertyChanged;

namespace Laundry.Views
{
  [AddINotifyPropertyChangedInterface]
  public class EmployeeCardViewModel : Card<Employee>
  {
    public EmployeeCardViewModel(IEventAggregator eventAggregator, OrderDataGridViewModel orderGrid,
      Visibilities visibilities) : base(eventAggregator, Screens.EmployeeEditor, visibilities)
    {
      this.OrderGrid = orderGrid;
      orderGrid.DisplaySelectionColumn = false;
      orderGrid.IsCompact = true;
      this._eventAggregator = eventAggregator;
    }

    public override Employee Entity
    {
      get { return base.Entity; }

      set
      {
        base.Entity = value;
        this.OrderGrid.Filter = Builders<Order>.Filter.And(
          Builders<Order>.Filter.Exists(nameof(Order.DeletionDate), false),
          Builders<Order>.Filter.Or(
            Builders<Order>.Filter.Eq(nameof(Order.ObtainerId), value.Id),
            Builders<Order>.Filter.Eq(nameof(Order.InCourierId), value.Id),
            Builders<Order>.Filter.Eq(nameof(Order.WasherCourierId), value.Id),
            Builders<Order>.Filter.Eq(nameof(Order.OutCourierId), value.Id),
            Builders<Order>.Filter.Eq(nameof(Order.DistributerId), value.Id)
          )
        );
        this.OrderGrid.Refresh(0, 10);
      }
    }

    public OrderDataGridViewModel OrderGrid { get; set; }

    public void ShowOrdersForClint()
    {
      this._eventAggregator.PublishOnUIThread(Screens.OrderDictionary);
      this._eventAggregator.PublishOnUIThread(this.Entity);
    }
  }
}