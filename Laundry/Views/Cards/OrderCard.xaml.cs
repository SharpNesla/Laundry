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
using MongoDB.Bson.Serialization.Attributes;
using PropertyChanged;

namespace Laundry.Views
{
  public class OrderCardViewModel : Card<Order>
  {
    private readonly IModel _model;
    public ClothDataGridViewModel ClothInstancesGrid { get; }

    public Client Client { get; private set; }

    public Employee InCourier { get; private set; }

    public Employee Obtainer { get; private set; }

    public Client CorpObtainer { get; private set; }

    public Employee Washer { get; private set; }

    public Employee OutCourier { get; private set; }

    public Client CorpDistributer { get; private set; }

    public Employee Distributer { get; private set; }

    public override Order Entity
    {
      get { return base.Entity; }

      set
      {
        base.Entity = value;
        this.ClothInstancesGrid.Order = value;
        this.ClothInstancesGrid.Refresh(0, 10);

        this.Client = _model.Clients.GetById(value.ClientId);
        InCourier = _model.Employees.GetById(value.InCourierId);

        Washer = _model.Employees.GetById(value.WasherCourierId);

        OutCourier = _model.Employees.GetById(value.OutCourierId);
        
        if (value.IsCorporative)
        {
          CorpObtainer = _model.Clients.GetById(value.CorpObtainerId);
          CorpDistributer = _model.Clients.GetById(value.CorpDistributerId);
        }
        else
        {

          Obtainer = _model.Employees.GetById(value.ObtainerId);
          Distributer = _model.Employees.GetById(value.DistributerId);
        }
      }
    }

    public OrderCardViewModel(IModel model, IEventAggregator eventAggregator, ClothDataGridViewModel clothGrid) : base(
      eventAggregator, Screens.OrderEditor)
    {
      _model = model;
      this.ClothInstancesGrid = clothGrid;
      clothGrid.DisplaySelectionColumn = false;
    }
  }
}