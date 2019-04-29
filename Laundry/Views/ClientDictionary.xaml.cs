using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using Caliburn.Micro;
using Laundry.Model;
using Laundry.Utils;
using Laundry.Utils.Controls;
using MaterialDesignThemes.Wpf;

namespace Laundry.Views
{
  public class ClientDictionaryViewModel : DictionaryScreen<ClientDataGridViewModel>
  {
    public ClientDictionaryViewModel(IEventAggregator aggregator, IModel model, PaginatorViewModel paginator,
      ClientDataGridViewModel entityGrid) :
      base(aggregator, model, paginator, entityGrid, "Клиентов")
    {
    }
  }
}