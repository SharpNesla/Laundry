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

namespace Laundry.Views
{
  /// <summary>
  /// Interaction logic for SubsidiaryEditor.xaml
  /// </summary>
  public class SubsidiaryEditorViewModel : EditorScreen<Repository<Subsidiary>, Subsidiary>
  {
    public SubsidiaryEditorViewModel(IEventAggregator aggregator, IModel model, Repository<Subsidiary> entityRepo, 
      string entityTitleName = "Филиала") : base(aggregator, model, entityRepo, entityTitleName)
    {
    }
  }
}