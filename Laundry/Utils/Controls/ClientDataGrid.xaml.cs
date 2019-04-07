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
using Laundry.Model.DatabaseClients;
using Laundry.Views;

namespace Laundry.Utils.Controls
{
  /// <summary>
  /// Interaction logic for ClientDataGrid.xaml
  /// </summary>
  public class ClientDataGridViewModel : EntityGrid<Client, ClientRepository, ClientCardViewModel>
  {
    public ClientDataGridViewModel(IEventAggregator eventAggregator, ClientCardViewModel card,
      DeleteDialogViewModel deleteDialog, IModel model) :
      base(eventAggregator, card, model.Clients, deleteDialog, Screens.ClientEditor)
    {
    }
  }
}