﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Model.DatabaseClients;
using Laundry.Utils;
using Laundry.Utils.Controls;
using MongoDB.Driver;
using PropertyChanged;

namespace Laundry.Views
{
  /// <summary>
  /// Interaction logic for ClientEditor.xaml
  /// </summary>
  public class ClientEditorViewModel : EditorScreen<ClientRepository, Client>
  {
    public bool IsOrdersEnabled
    {
      get { return !IsNew; }
    }

    #region TabBindings
    
    public bool InfoChecked { get; set; }
    
    public bool OrderChecked { get; set; }

    #endregion

    public OrderDataGridViewModel OrderDataGrid { get; set; }
    public PaginatorViewModel Paginator { get; set; }

    public ClientEditorViewModel(IEventAggregator aggregator, IModel model, OrderDataGridViewModel grid,
      PaginatorViewModel paginator) : base(
      aggregator, model, model.Clients, "клиента")
    {
      InfoChecked = true;
      OrderDataGrid = grid;

      Paginator = paginator;
      Paginator.ElementsName = "Заказов";
      Paginator.RegisterPaginable(OrderDataGrid);
    }

    protected override void OnActivate()
    {
      base.OnActivate();
      Paginator.RefreshPaginable();
    }

    public void AddOrder(object sender, RoutedEventArgs e)
    {
      OrderDataGrid.Add();
      EventAggregator.BeginPublishOnUIThread(Entity);
    }

    public override void Handle(Client client)
    {
      base.Handle(client);
      OrderDataGrid.Filter = Builders<Order>.Filter.And(
        Builders<Order>.Filter.Exists(nameof(Order.DeletionDate), false),
        Builders<Order>.Filter.Eq(nameof(Order.ClientId), client.Id)
      );
      Paginator.RefreshPaginable();
    }

    public void ApplyChanges(ClientEditorView view)
    {
      view.PhoneNumber.GetBindingExpression(TextBox.TextProperty)?.UpdateSource();

      if (!Validation.GetHasError(view.PhoneNumber))
      {
        base.ApplyChanges();
      }
    }
  }
}