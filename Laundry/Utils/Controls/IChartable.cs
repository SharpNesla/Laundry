using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Laundry.Model.DatabaseClients;
using LiveCharts;

namespace Laundry.Utils.Controls
{
  public interface IChartable<out TEntity> : IEntityGrid<TEntity>, INotifyPropertyChanged where TEntity : IRepositoryElement
  {
    SeriesCollection Values { get; }
    string[] Labels { get; }
    string LabelsTitle { get; }
    string ValuesTitle { get; }
  }
}
