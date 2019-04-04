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
using Laundry.Utils.Controls;
using MaterialDesignThemes.Wpf;

namespace Laundry.Views
{
  public class OrderDictionaryViewModel : DrawerActivityScreen
  {

    public OrderDataGridViewModel OrdersViewModel { get; private set; }

    public PaginatorViewModel Paginator { get; set; }

    public bool IsSearchDrawerOpened { get; set; }

    public async void ShowOrderInfo(Order context)
    {
      
    }

    public OrderDictionaryViewModel(IEventAggregator aggregator, IModel model, PaginatorViewModel paginator, OrderDataGridViewModel orderGrid) : base(aggregator, model)
    {
      orderGrid.Entities = Model.Orders.Get(0, 0);


      this.Paginator = paginator;
      this.Paginator.ElementsName = "�������";
      this.Paginator.ElementsPerPage = 5;

      this.OrdersViewModel = orderGrid;

      this.Paginator.Changed += RefreshOrders;
    }

    protected override void OnActivate()
    {
      base.OnActivate();
      Paginator.Count = Model.Orders.GetCount();
      RefreshOrders(this.Paginator.CurrentPage - 1, this.Paginator.ElementsPerPage);
    }

    private void RefreshOrders(int page, int elements)
    {
      this.OrdersViewModel.Entities = Model.Orders.Get(page * elements, elements);
    }

    public void ChangeSearchDrawerState()
    {
      IsSearchDrawerOpened = !IsSearchDrawerOpened;
    }
  }
}