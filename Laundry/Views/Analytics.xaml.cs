using System;
using System.ComponentModel;
using Caliburn.Micro;
using Laundry.Model;
using Laundry.Model.DatabaseClients;
using Laundry.Utils;
using Laundry.Utils.Controls;
using LiveCharts;
using LiveCharts.Wpf;

namespace Laundry.Views
{
  public enum ChartType
  {
    [Description("Столбчатая")]
    LineChart,
    [Description("Линейная")]
    Column,
    [Description("Круговая")]
    PieChart
  }

  public enum AnalyticEntityType
  {
    [Description("Заказы")]
    Order,
    [Description("Филиалы")]
    Subsidiary,
    [Description("Виды одежды")]
    ClothKind
  }

  public class AnalyticsViewModel : DrawerActivityScreen
  {
    private readonly OrderDataGridViewModel _orderGrid;
    private readonly SubsidiaryGridViewModel _subsidiaryGrid;
    private readonly ClothKindGridViewModel _clothKindGrid;
    private AnalyticEntityType _entityType;
    public IEntityGrid<IRepositoryElement> EntityGrid { get; set; }
    public PaginatorViewModel Paginator { get; set; }

    public bool IsGridChecked { get; set; }
    public bool IsChartChecked { get; set; }

    public ChartType ChartType { get; set; }

    public AnalyticEntityType EntityType
    {
      get { return _entityType; }
      set
      {
        _entityType = value;
        switch (value)
        {
          case AnalyticEntityType.Order:
            ChangeEntity(_orderGrid, "Заказов");
            break;
          case AnalyticEntityType.Subsidiary:
            ChangeEntity(_subsidiaryGrid, "Филиалов");
            break;
          case AnalyticEntityType.ClothKind:
            ChangeEntity(_clothKindGrid, "Видов одежды");
            break;
        }
      }
    }

    private void ChangeEntity(IEntityGrid<IRepositoryElement> repository, string elementsName)
    {
      Paginator.ClearPaginable();
      Paginator.RegisterPaginable(repository);
      Paginator.ElementsName = elementsName;

      this.EntityGrid = repository;

      Paginator.RefreshPaginable();
    }

    public SeriesCollection SeriesCollection { get; private set; }
    public string[] Labels { get; private set; }
    public Func<double, string> Formatter { get; private set; }

    public AnalyticsViewModel(IEventAggregator aggregator, IModel model, PaginatorViewModel paginator,
      OrderDataGridViewModel orderGrid, SubsidiaryGridViewModel subsidiaryGrid, ClothKindGridViewModel clothKindGrid) : base(aggregator, model)
    {
      _orderGrid = orderGrid;
      _orderGrid.IsDisplaySubtotals = true;
      _subsidiaryGrid = subsidiaryGrid;
      _subsidiaryGrid.IsDisplaySubtotals = true;
      _clothKindGrid = clothKindGrid;
      _clothKindGrid.IsDisplaySubtotals = true;

      Paginator = paginator;

      ChangeEntity(_orderGrid, "Заказов");

      this.IsGridChecked = true;
      
      SeriesCollection = new SeriesCollection
      {
        new ColumnSeries
        {
          Title = "2015",
          Values = new ChartValues<double> { 10, 50, 39, 50 }
        }
      };

      //adding series will update and animate the chart automatically
      SeriesCollection.Add(new ColumnSeries
      {
        Title = "2016",
        Values = new ChartValues<double> { 11, 56, 42 }
      });

      //also adding values updates and animates the chart automatically
      SeriesCollection[1].Values.Add(48d);

      Formatter = value => value.ToString("N");
    }
  }
}