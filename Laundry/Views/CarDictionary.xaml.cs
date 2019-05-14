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

namespace Laundry.Views
{
  /// <summary>
  /// Interaction logic for CarDictionary.xaml
  /// </summary>
  public class CarDictionaryViewModel : DictionaryScreen<CarDataGridViewModel>
  {
    public CarDictionaryViewModel(IEventAggregator aggregator, IModel model, PaginatorViewModel paginator, CarDataGridViewModel entityGrid) :
      base(aggregator, model, paginator, entityGrid, "Машин")
    {
    }
  }
}