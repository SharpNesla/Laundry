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
using Laundry.Model;
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