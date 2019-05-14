using Caliburn.Micro;
using Model;
using Laundry.Utils;
using Laundry.Utils.Controls;

namespace Laundry.Views
{
  /// <summary>
  /// Interaction logic for ClothKindEditor.xaml
  /// </summary>
  public class ClothKindDictionaryViewModel : DictionaryScreen<ClothKindTreeViewModel>
  {
    public bool IsTreeMode
    {
      get { return EntityGrid.IsTreeMode; }
      set { EntityGrid.IsTreeMode = value; }
    }

    public bool IsGridMode { get; set; }

    public ClothKindDictionaryViewModel(IEventAggregator aggregator, IModel model, PaginatorViewModel paginator, ClothKindTreeViewModel entityGrid) : 
      base(aggregator, model, paginator, entityGrid, "Видов одежды")
    {
      this.EntityGrid.IsTreeMode = true;
    }
  }
}