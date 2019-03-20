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
using Laundry.Utils;
using MaterialDesignThemes.Wpf;
using PropertyChanged;

namespace Laundry.Views
{
  [AddINotifyPropertyChangedInterface]
  public partial class ClientCard
  {
    private readonly IEventAggregator _eventAggregator;
    private Client _client;

    public Client Client
    {
      get { return _client; }
      set
      {
        _client = value;
        this.DataContext = value;
      }
    }

    public ClientCard(IEventAggregator eventAggregator)
    {
      InitializeComponent();
      _eventAggregator = eventAggregator;
      
    }

    private void EditClient(object sender, RoutedEventArgs e)
    {
      _eventAggregator.PublishOnUIThread(Screens.ClientEditor);
      _eventAggregator.PublishOnUIThread(this.Client);
    }
  }
}