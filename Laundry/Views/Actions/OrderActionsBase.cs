using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using Laundry.Model;
using Laundry.Model.CollectionRepositories;
using Laundry.Utils;
using Laundry.Utils.Controls;
using MongoDB.Driver;

namespace Laundry.Views.Actions
{
  public abstract class OrderActionsBase : Screen
  {
    protected readonly OrderRepository Repository;
    private readonly OrderStatus _changingStatus;
    public OrderDataGridViewModel OrderGrid { get; set; }

    public OrderActionsBase(OrderRepository orderRepo, Employee currentUser, OrderDataGridViewModel orderGrid,
      OrderStatus startStatus, OrderStatus changingStatus)
    {
      Repository = orderRepo;
      _changingStatus = changingStatus;
      OrderGrid = orderGrid;
      OrderGrid.Filter =
        Builders<Order>.Filter.Eq(nameof(Order.Status), startStatus);
      this.OrderGrid.Refresh(0, int.MaxValue);
    }

    public virtual void Apply()
    {
      this.Repository.SetOrdersStatus(this.OrderGrid.SelectedEntities, _changingStatus);
    }

    public abstract void PrintReport();
  }
}