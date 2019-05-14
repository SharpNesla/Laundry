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

namespace Laundry.Utils.Controls.EntitySearchControls
{
  public class ClothKindSelectorViewModel : PropertyChangedBase
  {
    public List<ClothKind> Tree { get; }
    public ClothKind SelectedEntity { get; set; }
    public Action<ClothKind> EntityChanged;
    public ClothKindSelectorViewModel(IModel model)
    {
      this.Tree = new List<ClothKind> {model.ClothKinds.GetFullTree()};
    }

    public void SelectionChanged(TreeView view)
    {
      this.SelectedEntity = view.SelectedItem as ClothKind;
      EntityChanged?.Invoke(SelectedEntity);
    }
  }
}