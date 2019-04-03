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
using PropertyChanged;

namespace Laundry.Utils.Controls
{
  /// <summary>
  /// Interaction logic for ClientSearch.xaml
  /// </summary>
  public class ClientSearchViewModel : PropertyChangedBase
  {
    private IModel _model;
    private Client _selectedClient;
    public IList<Client> Clients { get; set; }

    public Client SelectedClient
    {
      get { return _selectedClient; }
      set
      {
        _selectedClient = value;
        ClientChanged?.Invoke(value);
      }
    }

    public event Action<Client> ClientChanged;
    public ClientSearchViewModel(IModel model)
    {
      this._model = model;
      this.Clients = model.Clients.Get(0, 10);
    }

    public void OnTextInput(ComboBox box)
    {
      box.IsDropDownOpen = true;
    }
  }
}