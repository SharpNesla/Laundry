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
using Laundry.Views.Actions;
using LiveCharts;
using LiveCharts.Wpf;
using Model.CollectionRepositories;
using MongoDB.Driver;
using PropertyChanged;

namespace Laundry.Views.Dashboards
{
  public class DirectorDashBoardViewModel : DashBoardBase
  {
    public DirectorDashBoardViewModel(IEventAggregator aggregator, IModel model,
      OrderDataGridViewModel orderGrid, OrderDataGridViewModel actionsOrderGrid) : base(
      aggregator, model, orderGrid, actionsOrderGrid)
    {
    }

    #region Св-ва, отражающие состояние предприятия

    public long EmployeeCount => this.Model.Employees.GetCount();

    #endregion

    public long OrdersCountByMounth => this.Model.Orders.GetCount(
      Builders<Order>.Filter.Gte(nameof(Order.ExecutionDate), DateTime.Now.AddMonths(-1)));

    public string AggregatedInstancesCountByMounth => this.Model.Orders.GetAggregatedInstacesCount(
      Builders<Order>.Filter.Gte(nameof(Order.ExecutionDate), DateTime.Now.AddMonths(-1)));

    public double AggregatedPriceByMounth => this.Model.Orders.GetAggregatedPrice(
      Builders<Order>.Filter.Gte(nameof(Order.ExecutionDate), DateTime.Now.AddMonths(-1)));
    
    #region Св-ва относящиеся к графикам за последний месяц

    public SeriesCollection MoneyValues => new SeriesCollection
    {
      new ColumnSeries
      {
        Title = "₽",
        Values = new ChartValues<double>(this.AggregationResults.Select(x => x.Price))
      }
    };

    public SeriesCollection ThingsValues => new SeriesCollection
    {
      new ColumnSeries
      {
        Title = "шт",
        Values = new ChartValues<long>(this.AggregationResults.Select(x => x.Count))
      },
      new ColumnSeries
      {
        Title = "кг",
        Values = new ChartValues<double>(this.AggregationResults.Select(x => x.UnCountableCount))
      }
    };

    public IReadOnlyList<AggregationResult> AggregationResults => this.Model.Orders.AggregateOrders(Time,
      Builders<Order>.Filter.Gte(nameof(Order.ExecutionDate), DateTime.Now.AddMonths(-1)));

    public string[] DayLabels => this.AggregationResults.Select(x => x.DateTime.ToString("d")).ToArray();

    [AlsoNotifyFor(nameof(DayLabels))]
    public ChartTime Time { get; set; }

    #endregion
  }
}