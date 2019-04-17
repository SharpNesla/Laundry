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
using Laundry.Views.Cards;
using PropertyChanged;

namespace Laundry.Utils.Controls
{
  /// <summary>
  /// Interaction logic for OrderDataGrid.xaml
  /// </summary>
  
  public class ClothDataGridViewModel : EntityGrid<ClothInstance, ClothInstancesRepository, ClothInstanceCardViewModel>
  {
    public bool IsNewOrder { get; set; }
    public Order Order { get; set; }

    public ClothDataGridViewModel(IEventAggregator eventAggregator, ClothInstanceCardViewModel card, IModel model,
      DeleteDialogViewModel shure) : base(eventAggregator, card, model.ClothInstances, shure, Screens.About)
    {
    }

    public override void ExportToExcel()
    {
      throw new NotImplementedException();
    }
  }
}
