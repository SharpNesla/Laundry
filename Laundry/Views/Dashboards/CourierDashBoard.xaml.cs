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

namespace Laundry.Views.Dashboards
{
  public class CourierDashBoardViewModel : DashBoardBase
  {
    public CourierDashBoardViewModel(IEventAggregator aggregator, IModel model) : base(aggregator, model)
    {
    }
  }
}
