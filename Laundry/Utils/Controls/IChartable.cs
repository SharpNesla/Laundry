using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.DatabaseClients;
using Laundry.Views;
using LiveCharts;

namespace Laundry.Utils.Controls
{
  /// <summary>
  /// Интерфейс описывающий сущноcть,
  /// которую можно использовать для построения графиков
  /// по видам одежды
  /// </summary>
  /// <typeparam name="TEntity">Тип сущности</typeparam>
  public interface IChartable<out TEntity> : IEntityGrid<TEntity>, INotifyPropertyChanged
    where TEntity : RepositoryElement
  {
    /// <summary>
    /// Значения по оси y
    /// </summary>
    SeriesCollection Values { get; }

    /// <summary>
    /// Подписи по оси x
    /// </summary>
    string[] Labels { get; }

    /// <summary>
    /// Промежуток времени
    /// </summary>
    ChartTime Time { get; set; }

    /// <summary>
    /// Вид информации об одежде
    /// </summary>
    EntityInfoType EntityInfoType { get; set; }
  }
}