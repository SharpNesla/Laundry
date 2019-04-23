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
    private readonly IModel _model;
    public Order Order { get; set; }
    
    public ClothDataGridViewModel(IEventAggregator eventAggregator,
      ClothInstanceCardViewModel card, IModel model,
      DeleteDialogViewModel shure) : base(eventAggregator, card, model.ClothInstances, shure, Screens.About)
    {
      _eventAggregator = eventAggregator;
      _model = model;
    }

    public override async void Add()
    {
      var editor = new ClothEditorViewModel(_eventAggregator, _model, this.Order);
      await DialogHostExtensions.ShowCaliburnVM(editor);
      RaiseStateChanged();
    }

    public override async void Edit()
    {
      var editor = new ClothEditorViewModel(_eventAggregator, _model, this.Order);

      _eventAggregator.PublishOnUIThread(SelectedEntity);
      await DialogHostExtensions.ShowCaliburnVM(editor);
      RaiseStateChanged();
    }

    public override void Refresh(int page, int elements)
    {
      this.Entities = Order.Instances.Skip(page * elements).Take(elements).ToList().AsReadOnly();
    }

    public override long Count
    {
      get { return Order.Instances.Count; }
    }

    public override void ExportToExcel()
    {
      throw new NotImplementedException();
    }
  }
}