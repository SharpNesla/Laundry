using Caliburn.Micro;
using Laundry.Model;
using Laundry.Utils;

namespace Laundry.Views
{
  public class SubsidiaryDictionaryViewModel : DrawerActivityScreen
  {
    public SubsidiaryDictionaryViewModel(IEventAggregator aggregator, IModel model) : base(aggregator, model)
    {
    }

    public void AddSubsidiary()
    {
      ChangeApplicationScreen(Screens.SubsidiaryEditor);
    }
  }
}