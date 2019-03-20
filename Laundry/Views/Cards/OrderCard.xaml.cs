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
using Laundry.Model;
using Laundry.Utils;
using MaterialDesignThemes.Wpf;
using PropertyChanged;

namespace Laundry.Views
{
  [AddINotifyPropertyChangedInterface]
  public partial class OrderCard
  {
    private readonly IEventAggregator _eventAggregator;
    private Order _order;

    public Order Order
    {
      get { return _order; }
      set
      {
        _order = value;
        this.DataContext = value;
      }
    }

    public OrderCard(IEventAggregator eventAggregator)
    {
      InitializeComponent();
      _eventAggregator = eventAggregator;

    }

    private void EditOrder(object sender, RoutedEventArgs e)
    {
      _eventAggregator.PublishOnUIThread(Screens.ClientEditor);
      _eventAggregator.PublishOnUIThread(this.Order);
    }
  }
}
