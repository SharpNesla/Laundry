using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using Laundry.Model;
using Laundry.Utils;

namespace Laundry.Views.Dashboards
{
  public class DashBoardBase : DrawerActivityScreen
  {
    public DashBoardBase(IEventAggregator aggregator, IModel model) : base(aggregator, model)
    {
    }
  }
}
