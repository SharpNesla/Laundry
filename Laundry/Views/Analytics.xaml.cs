using Caliburn.Micro;
using Laundry.Model;
using Laundry.Utils;

namespace Laundry.Views
{
  public class AnalyticsViewModel : DrawerActivityScreen
  {
    public AnalyticsViewModel(IEventAggregator aggregator, IModel model) : base(aggregator, model)
    {
    }
  }
}