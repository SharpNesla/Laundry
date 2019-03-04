using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Caliburn.Micro;
using Laundry.Model;
using Laundry.Utils;
using MaterialDesignThemes.Wpf;

namespace Laundry.Views
{
  /// <summary>
  /// Interaction logic for DashBoard.xaml
  /// </summary>
  public class EmployeeEditorViewModel : DrawerActivityScreen
  {
    
    public EmployeeEditorViewModel(IEventAggregator aggregator, IModel model) : base(aggregator, model)
    {
      
    }

    public void Cancel()
    {
      ChangeApplicationScreen(Screens.Context);
    }
  }
}