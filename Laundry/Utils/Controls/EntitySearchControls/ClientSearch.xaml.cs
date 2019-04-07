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

    public override void OnEntitySearch(string entityText)
    {
      this.Entities = new List<Client>(this.Repository.Get(0, 10,
        Builders<Client>.Filter.Regex(nameof(Client.Name), new MongoDB.Bson.BsonRegularExpression($@"{entityText}\w*"))));
    }
  }
}