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
  public class ClientCardViewModel : Card<Client>
  {
    public override Client Entity
    {
      get { return base.Entity; }

      set
      {
        base.Entity = value;
        this.OrderGrid.Filter = Builders<Order>.Filter.And(
          Builders<Order>.Filter.Exists(nameof(Order.DeletionDate), false),
          Builders<Order>.Filter.Eq(nameof(Order.ClientId), value.Id)
        );
        this.OrderGrid.Refresh(0, 10);
      }
    }

    public OrderDataGridViewModel OrderGrid { get; set; }

    public ClientCardViewModel(IEventAggregator eventAggregator, OrderDataGridViewModel orderGrid,
      Visibilities visibilities = null) :
      base(eventAggregator, Screens.ClientEditor, visibilities)
    {
      this.OrderGrid = orderGrid;
      orderGrid.DisplaySelectionColumn = false;
      orderGrid.IsCompact = true;
      this._eventAggregator = eventAggregator;
    }

    public void ShowOrdersForClint()
    {
      this._eventAggregator.PublishOnUIThread(Screens.OrderDictionary);
      this._eventAggregator.PublishOnUIThread(this.Entity);
    }
  }
}