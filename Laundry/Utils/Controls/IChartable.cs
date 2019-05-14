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
  public interface IChartable<out TEntity> : IEntityGrid<TEntity>, INotifyPropertyChanged
    where TEntity : IRepositoryElement
  {
    SeriesCollection Values { get; }
    string[] Labels { get; }
    ChartTime Time { get; set; }
    EntityInfoType EntityInfoType { get; set; }
  }
}