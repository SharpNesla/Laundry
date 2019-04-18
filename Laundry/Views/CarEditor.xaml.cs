using Caliburn.Micro;
using Laundry.Model;
using Laundry.Model.CollectionRepositories;
using Laundry.Utils;
using Laundry.Utils.Controls;
using PropertyChanged;

namespace Laundry.Views
{
  /// <summary>
  /// Interaction logic for CarEditor.xaml
  /// </summary>
  public class CarEditorViewModel : EditorScreen<Repository<Car>, Car>
  {
    public CarEditorViewModel(IEventAggregator aggregator, IModel model) : base(aggregator, model, model.Cars, "автомобиля")
    {
    }
  }
}