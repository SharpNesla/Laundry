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
using Laundry.Utils;
using Action = System.Action;

namespace Laundry.Views
{
  /// <summary>
  /// Interaction logic for ClothEditor.xaml
  /// </summary>
  public class ClothEditorViewModel : EditorScreen<ClothInstancesRepository, ClothInstance>
  {
    public Order Order { get; set; }
    public bool IsNewOrder { get; set; }

    public event Action Created;

    public ClothEditorViewModel(IEventAggregator aggregator, IModel model) :
      base(aggregator, model, model.ClothInstances, "предмета одежды")
    {
    }

    public override void ApplyChanges()
    {
      
      if (IsNewOrder)
      {
        EntityRepository.AddUnRegistred(this.Entity);
      }
      else
      {
        if (IsNew)
        {
          EntityRepository.SetOrder(this.Entity, this.Order);
          EntityRepository.Add(this.Entity);
        }
        else
        {
          EntityRepository.Update(this.Entity);
        }
      }

      Created?.Invoke();
    }
  }
}