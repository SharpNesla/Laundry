using System;
using Caliburn.Micro;
using Laundry.Model;
using Laundry.Utils;

namespace Laundry.Views
{
  /// <summary>
  /// Interaction logic for OrderEditor.xaml
  /// </summary>
  public class OrderEditorViewModel : ActivityScreen, IHandle<Order>
  {
    public DateTime DateTime { get; set; }
    public BindableCollection<ClothInstance> ClothInstances { get; set; }

    public OrderEditorViewModel(IEventAggregator aggregator, IModel model, ClothEditor editor) : base(aggregator, model)
    {
      //  var kind = new ClothKind {MeasureKind = MeasureKind.Kg, Name = "Носки"};

      //  this.ClothInstances = new BindableCollection<ClothInstance>(
      //    new[]
      //    {
      //      new ClothInstance {Amount = 3, Kind = kind, WearPercentage = 0},
      //      new ClothInstance {Amount = 3, Kind = kind, WearPercentage = 0},
      //      new ClothInstance {Amount = 3, Kind = kind, WearPercentage = 0},
      //      new ClothInstance {Amount = 3, Kind = kind, WearPercentage = 0},
      //      new ClothInstance {Amount = 3, Kind = kind, WearPercentage = 0},
      //    }
      //  );
    }

    public void Cancel()
    {
      ChangeApplicationScreen(Screens.Context);
    }

    public void Handle(Order message)
    {
      throw new NotImplementedException();
    }
  }
}