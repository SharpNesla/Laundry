using System;
using System.Collections.Generic;
using System.Windows.Controls;
using Caliburn.Micro;
using Model;
using Model.DatabaseClients;
using MongoDB.Driver;

namespace Laundry.Utils.Controls.EntitySearchControls
{
  /// <summary>
  /// Interaction logic for ClientSearch.xaml
  /// </summary>
  public class ClientSearchViewModel : EntitySearchBox<Client, ClientRepository>
  {
    public ClientSearchViewModel(IModel model, string label = "Клиент", FilterDefinition<Client> filter = null) : base(model.Clients, label, filter)
    {
    }
  }
}