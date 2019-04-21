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
using Laundry.Model;
using Laundry.Model.CollectionRepositories;
using MongoDB.Driver;

namespace Laundry.Utils.Controls.EntitySearchControls
{
  public class SubsidiarySearchViewModel : EntitySearchBox<Subsidiary, SubsidiaryRepository>
  {
    public SubsidiarySearchViewModel(IModel model, string label = "Филиал", FilterDefinition<Subsidiary> filter = null) : base(model.Subsidiaries, label, filter)
    {
    }
  }
}
