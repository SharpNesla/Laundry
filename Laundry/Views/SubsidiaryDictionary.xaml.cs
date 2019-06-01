using Caliburn.Micro;
using Model;
using Laundry.Utils;
using Laundry.Utils.Controls;

namespace Laundry.Views
{
  public class SubsidiaryDictionaryViewModel : DictionaryScreen<SubsidiaryGridViewModel>
  {
    public SubsidiaryDictionaryViewModel(IEventAggregator aggregator, IModel model,
      PaginatorViewModel paginator, SubsidiaryGridViewModel entityGrid) 
      : base(aggregator, model, paginator, entityGrid, "Филиалов")
    {
    }
  }
}