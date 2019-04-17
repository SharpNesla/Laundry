using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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

namespace Laundry.Utils.Controls
{

  /// <summary>
  /// Interaction logic for ClothKindGrid.xaml
  /// </summary>
  public class ClothKindGridViewModel : EntityGrid<ClothKind, ClothKindRepository, ClothKindCardViewModel>
  {
    public float NameWidth { get; set; }

    public ClothKindGridViewModel(IEventAggregator eventAggregator, ClothKindCardViewModel card,
      IModel model, DeleteDialogViewModel shure)
      : base(eventAggregator, card, model.ClothKinds, shure, Screens.About)
    {
    }

    public void ShowHideDetails(ToggleButton button, ClothKind clothKind)
    {
      for (var vis = button as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
      {
        if (vis is DataGridRow)
        {
          var row = (DataGridRow)vis;
          row.DetailsVisibility =
            row.DetailsVisibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
          break;
        }
      }

      this.Repo.FetchChildren(clothKind);
    }

    public override void Refresh(int page, int elements)
    {
      this.Entities = new[] {Repo.GetById(0)};
    }

    public override void ExportToExcel()
    {
      throw new NotImplementedException();
    }

    public void CheckBranch(ToggleButton button)
    {
    }
  }
}