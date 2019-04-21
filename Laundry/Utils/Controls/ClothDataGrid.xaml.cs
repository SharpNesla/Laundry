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
    private readonly IEventAggregator _eventAggregator;
    private readonly ClothEditorViewModel _editor;
    public Order Order { get; set; }


    public bool IsNewOrder { get; set; }

    public ClothDataGridViewModel(IEventAggregator eventAggregator, ClothEditorViewModel editor,
      ClothInstanceCardViewModel card, IModel model,
      DeleteDialogViewModel shure) : base(eventAggregator, card, model.ClothInstances, shure, Screens.About)
    {
      _eventAggregator = eventAggregator;
      _editor = editor;
      _editor.Changed += this.RaiseStateChanged;
    }

    public override void Add()
    {

      if (IsNewOrder)
      {
        _editor.IsNewOrder = true;
      }
      else
      {
        _editor.Order = this.Order;
      }
      DialogHostExtensions.ShowCaliburnVM(_editor);
    }

    public override void Edit()
    {
      DialogHostExtensions.ShowCaliburnVM(_editor);
      _eventAggregator.PublishOnUIThread(SelectedEntity);

    }

    public override void Refresh(int page, int elements)
    {
      if (Order != null && !IsNewOrder)
      {
        this.Entities = Repo.GetForOrder(page * elements, elements, Order);
      }
      else
      {
        this.Entities = Repo.GetUnRegistred(page * elements, elements);
      }
    }

    public override long Count
    {
      get
      {
        if (Order != null && !IsNewOrder)
        {
          return Repo.GetForOrderCount(this.Order);
        }
        else
        {
          return Repo.GetUnRegistredCount();
        }
      }
    }

    public override void ExportToExcel()
    {
      throw new NotImplementedException();
    }
  }
}