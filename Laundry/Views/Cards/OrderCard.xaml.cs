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

namespace Laundry.Views
{
  public partial class OrderCard
  {
    private readonly IEventAggregator _eventAggregator;
    public Client Client { get; set; }

    public OrderCard(IEventAggregator eventAggregator)
    {
      _eventAggregator = eventAggregator;
      InitializeComponent();
      this.DataContext = this;
    }

    private void EditClient(object sender, RoutedEventArgs e)
    {
      _eventAggregator.PublishOnUIThread(Screens.ClientEditor);
      _eventAggregator.PublishOnUIThread(this.Client);
      
    }
  }
}