using System;
using System.ComponentModel;
using Caliburn.Micro;
using Model;
using Model.DatabaseClients;
using Laundry.Utils;
using Laundry.Utils.Controls;
using LiveCharts;
using LiveCharts.Wpf;

namespace Laundry.Views
{
  public enum ChartTime
  {
    [Description("День")] Day,
    [Description("Месяц")] Mounth,
    [Description("Год")] Year
  }

  public enum EntityInfoType
  {
    [Description("Количество")] Amount,
    [Description("Суммарная стоимость")] Cost
  }

  public enum AnalyticEntityType
  {
    [Description("Заказы")] Order,
    [Description("Филиалы")] Subsidiary,
    [Description("Виды одежды")] ClothKind
  }

  /// <summary>
  /// Форма аналитики заказов, филиалов, видов одежды
  /// </summary>
  public class AnalyticsViewModel : DrawerActivityScreen
  {
    #region Таблицы сущностей

    private readonly OrderDataGridViewModel _orderGrid;
    private readonly SubsidiaryGridViewModel _subsidiaryGrid;
    private readonly ClothKindTreeViewModel _clothKindGrid;

    #endregion

    private AnalyticEntityType _entityType;

    /// <summary>
    /// Отображаемая таблица
    /// </summary>
    public IChartable<RepositoryElement> EntityGrid { get; set; }

    public PaginatorViewModel Paginator { get; set; }

    public string SearchHintString { get; private set; }

    #region Флаги вкладок

    public bool IsGridChecked { get; set; }
    public bool IsChartChecked { get; set; }

    #endregion

    public ChartTime ChartTime
    {
      get { return this.EntityGrid.Time; }
      set { this.EntityGrid.Time = value; }
    }

    /// <summary>
    /// Свойство-переключатель текущей сущности
    /// </summary>
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

    /// <summary>
    /// Смена текущей сущности на другую
    /// </summary>
    /// <param name="chartable">Chartable таблица</param>
    /// <param name="elementsName">Название сущности</param>
    private void ChangeEntity(IChartable<RepositoryElement> chartable, string elementsName)
    {
      Paginator.CurrentPage = 1;
      Paginator.RegisterPaginable(chartable);
      Paginator.ElementsName = elementsName;

      this.SearchHintString = $"Поиск {elementsName.ToLower()}";

      this.EntityGrid = chartable;
    }


    protected override void OnActivate()
    {
      base.OnActivate();
      this.Paginator.RefreshPaginable();
    }

    public AnalyticsViewModel(IEventAggregator aggregator, IModel model, PaginatorViewModel paginator,
      OrderDataGridViewModel orderGrid, SubsidiaryGridViewModel subsidiaryGrid, ClothKindTreeViewModel clothKindGrid) :
      base(aggregator, model)
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

    public void ExportToExcel()
    {
      EntityGrid.ExportToExcel();
    }

    public void ExportToCSV()
    {
      EntityGrid.ExportToExcel();
    }
  }
}