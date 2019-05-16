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
using Model;
using Model.CollectionRepositories;
using Laundry.Utils;

namespace Laundry.Views
{
  /// <summary>
  /// Interaction logic for ClothEditor.xaml
  /// </summary>
  public class ClothKindEditorViewModel : EditorScreen<ClothKindRepository, ClothKind>
  {
    private ClothKind _clothKindParent;

    public ClothKindEditorViewModel(IEventAggregator aggregator, IModel model) :
      base(aggregator, model, model.ClothKinds, "предмета одежды")
    {
    }

    public ClothKind ClothKindParent
    {
      get { return _clothKindParent; }
      set
      {
        _clothKindParent = value;
        this.Entity.Parent = _clothKindParent.Id;
      }
    }

    public override void ApplyChanges()
    {
      if (IsNew)
      {
        EntityRepository.Add(this.Entity);
      }
      else
      {
        EntityRepository.Update(this.Entity);
      }


      DialogHostExtensions.CloseCurrent();
    }
  }
}