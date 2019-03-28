using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Caliburn.Micro;
using Laundry.Model;
using Laundry.Utils;
using MaterialDesignThemes.Wpf;
using PropertyChanged;

namespace Laundry.Views
{
  /// <summary>
  /// Interaction logic for DashBoard.xaml
  /// </summary>
  public class EmployeeEditorViewModel : DrawerActivityScreen
  {
    [AlsoNotifyFor(nameof(EditorTitle))]
    public Employee Employee { get; set; }

    public string EditorTitle
    {
      get { return Employee != null ? $"Редактирование работника №{Employee.Id}" : "Редактирование нового работника"; }
    }
    #region TabBindings

    [AlsoNotifyFor(nameof(InfoVisibility))]
    public bool InfoChecked { get; set; }

    [AlsoNotifyFor(nameof(OrderGridVisibility))]
    public bool OrderChecked { get; set; }


    public Visibility InfoVisibility
    {
      get { return InfoChecked ? Visibility.Visible : Visibility.Collapsed; }
    }

    public Visibility OrderGridVisibility
    {
      get { return OrderChecked ? Visibility.Visible : Visibility.Collapsed; }
    }

    #endregion

    public EmployeeEditorViewModel(IEventAggregator aggregator, IModel model) : base(aggregator, model)
    {
      this.InfoChecked = true;
    }

    public void ApplyChanges()
    {
      var isNewEmployee = this.Employee == null;

      if (isNewEmployee)
      {
        
      }

      ChangeApplicationScreen(Screens.Context);
    }

    public void Discard()
    {
      ChangeApplicationScreen(Screens.Context);
    }
  }
}