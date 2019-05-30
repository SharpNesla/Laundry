using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using Model;
using Laundry.Utils;
using Laundry.Utils.Controls;

namespace Laundry.Views.Dashboards
{
  /// <summary>
  /// Базовый класс для дашборда работника, наследуется от activity с drawer'ом
  /// </summary>
  public class DashBoardBase : DrawerActivityScreen
  {

    /// <summary>
    /// Таблица заказов
    /// </summary>
    public OrderDataGridViewModel OrderGrid { get; internal set; }

    /// <summary>
    /// Инстанс таблицы для использования в действиях
    /// </summary>
    protected readonly OrderDataGridViewModel ActionsOrderGrid;
    
    public DashBoardBase(IEventAggregator aggregator, IModel model, OrderDataGridViewModel orderGrid,
      OrderDataGridViewModel actionsGrid)
      : base(aggregator, model)
    {
      this.OrderGrid = orderGrid;
      this.ActionsOrderGrid = actionsGrid;
      orderGrid.DisplaySelectionColumn = false;
    }

    /// <summary>
    /// Скрываем область расширенного поиска при активации дашборда
    /// </summary>
    protected override void OnActivate()
    {
      base.OnActivate();
      OrderGrid.IsSearchDrawerOpened = false;
    }
  }
}