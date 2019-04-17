using System;
using System.Collections.Generic;
using System.Windows.Controls;
using Caliburn.Micro;
using Laundry.Model;
using Laundry.Model.CollectionRepositories;
using Laundry.Utils.Controls.EntitySearchControls;
using Laundry.Views;
using MaterialDesignThemes.Wpf;

namespace Laundry.Utils.Controls
{
  /// <summary>
  /// Interaction logic for OrderDataGrid.xaml
  /// </summary>
  public class OrderDataGridViewModel : EntityGrid<Order, OrderRepository, OrderCardViewModel>
  {

    

    public bool IsByCreationDate { get; set; }
    public bool IsByExecutionDate { get; set; }

    public bool IsByClient { get; set; }
    public bool IsCorporative { get; set; }
    public ClientSearchViewModel ClientCombo { get; set; }

    public bool IsByEmployee { get; set; }
    public EmployeeProfession Profession { get; set; }
    public EmployeeSearchViewModel EmployeeCombo { get; set; }

    public Client Client { get; set; }
    public Employee Employee { get; set; }

    public override void Refresh(int page, int elements)
    {
      if (Client != null)
      {
        this.Entities = Repo.GetForClient(Client, page * elements, elements);
        return;
      }

      if (Employee != null)
      {
        this.Entities = Repo.GetForEmployee(Employee, page * elements, elements);
      }
      else
        base.Refresh(page, elements);
    }

    public override void ExportToExcel()
    {
      throw new NotImplementedException();
    }

    public override long Count
    {
      get
      {
        if (Client != null)
          return Repo.GetForClientCount(Client);
        if (Employee != null)
        {
          return Repo.GetForEmployeeCount(Employee);
        }

        return base.Count;
      }
    }

    public OrderDataGridViewModel(IEventAggregator eventAggregator, OrderCardViewModel card,
      DeleteDialogViewModel deleteDialog, IModel model
    ) : base(eventAggregator, card, model.Orders, deleteDialog, Screens.OrderEditor)
    {
      this.ClientCombo = new ClientSearchViewModel(model);
      this.ClientCombo.EntityChanged += OnEntityChanged;

      this.EmployeeCombo = new EmployeeSearchViewModel(model) { Label = "Работник" };
    }


    private void OnEntityChanged(Client obj)
    {
      this.Client = obj;
    }
  }
}