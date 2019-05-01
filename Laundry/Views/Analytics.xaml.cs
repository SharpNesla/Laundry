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
    public IChartable<IRepositoryElement> EntityGrid { get; set; }
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

    private void ChangeEntity(IChartable<IRepositoryElement> repository, string elementsName)
    {
      Paginator.CurrentPage = 1;
      Paginator.ClearPaginable();
      Paginator.RegisterPaginable(repository);
      Paginator.ElementsName = elementsName;

      this.EntityGrid = repository;
      
      Paginator.RefreshPaginable();
    }

    protected override void OnActivate()
    {
      base.OnActivate();
      this.Paginator.RefreshPaginable();
    }

    public AnalyticsViewModel(IEventAggregator aggregator, IModel model, PaginatorViewModel paginator,
      OrderDataGridViewModel orderGrid, SubsidiaryGridViewModel subsidiaryGrid, ClothKindGridViewModel clothKindGrid) : base(aggregator, model)
    {
      _orderGrid = orderGrid;
      _orderGrid.IsDisplaySubtotals = true;
      _orderGrid.DisplaySelectionColumn = false;
      _subsidiaryGrid = subsidiaryGrid;
      _subsidiaryGrid.IsDisplaySubtotals = true;
      _subsidiaryGrid.DisplaySelectionColumn = false;
      _clothKindGrid = clothKindGrid;
      _clothKindGrid.IsDisplaySubtotals = true;
      _clothKindGrid.DisplaySelectionColumn = false;

      Paginator = paginator;
      ChangeEntity(_orderGrid, "Заказов");

      this.IsGridChecked = true;
    }

    public void AdvancedSearch()
    {
      this.Paginator.RefreshPaginable();
    }
  }
}