using System;
using System.Collections.Generic;
using System.Windows.Controls;
using Caliburn.Micro;
using Laundry.Model;
using Laundry.Model.DatabaseClients;
using MongoDB.Driver;

namespace Laundry.Utils.Controls.EntitySearchControls
{
  /// <summary>
  /// Interaction logic for ClientSearch.xaml
  /// </summary>
  public class ClientSearchViewModel : EntitySearchBox<Client, ClientRepository>
  {
    public ClientSearchViewModel(IModel model) : base(model.Clients)
    {
    }
  }
}