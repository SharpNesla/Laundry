using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using MaterialDesignThemes.Wpf;

namespace Laundry.Views
{
  public class OrderDictionaryViewModel : DictionaryScreen<OrderDataGridViewModel>
  {
    public OrderDictionaryViewModel(IEventAggregator aggregator, IModel model, PaginatorViewModel paginator, 
      OrderDataGridViewModel entityGrid) : base(aggregator, model, paginator, entityGrid, "Заказов")
    {
    }
  }
}