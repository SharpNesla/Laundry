using Caliburn.Micro;
using Laundry.Model;
using Laundry.Utils;
using Laundry.Utils.Controls;

namespace Laundry.Views
{
  public class SubsidiaryDictionaryViewModel : DrawerActivityScreen
  {
    public PaginatorViewModel Paginator { get; set; }

    public SubsidiaryDictionaryViewModel(IEventAggregator aggregator, IModel model, PaginatorViewModel paginator) : base(aggregator, model)
    {
      this.Paginator = paginator;
      paginator.ElementsName = "Филиалов";
    }

    public void AddSubsidiary()
    {
      ChangeApplicationScreen(Screens.SubsidiaryEditor);
    }
  }
}