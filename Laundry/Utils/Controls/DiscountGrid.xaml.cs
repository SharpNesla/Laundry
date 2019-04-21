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
using Laundry.Model.CollectionRepositories;
using Laundry.Views;

namespace Laundry.Utils.Controls
{
  public class DiscountGridViewModel : EntityGrid<DiscountEdge, DiscountSystemRepository, Card<DiscountEdge>>
  {
    public DiscountGridViewModel(IModel model, IEventAggregator eventAggregator, DeleteDialogViewModel deleteDialog) : base(eventAggregator, null, model.DiscountEdges, deleteDialog, Screens.About)
    {
    }

    public override void ExportToExcel()
    {
      
    }
  }
}
