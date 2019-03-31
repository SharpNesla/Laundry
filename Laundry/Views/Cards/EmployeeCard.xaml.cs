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
using Laundry.Utils.Controls;
using MaterialDesignThemes.Wpf;
using PropertyChanged;

namespace Laundry.Views
{
  [AddINotifyPropertyChangedInterface]
  public class EmployeeCardViewModel : IHandle<Employee>
  {
    private readonly IEventAggregator _eventAggregator;
    private IModel _model;
    public OrderDataGridViewModel OrderGrid { get; set; }
    public Employee Client { get; set; }

    public EmployeeCardViewModel(IEventAggregator eventAggregator, IModel model, OrderDataGridViewModel grid)
    {
      _eventAggregator = eventAggregator;
      eventAggregator.Subscribe(this);

      this.OrderGrid = grid;
      this._model = model;
    }

    private void EditClient(object sender, RoutedEventArgs e)
    {
      _eventAggregator.PublishOnUIThread(Screens.ClientEditor);
      _eventAggregator.PublishOnUIThread(this.Client);
    }

    public void Handle(Employee message)
    {
      this.Client = _model.Employees.GetById(message.Id);
    }
  }
}