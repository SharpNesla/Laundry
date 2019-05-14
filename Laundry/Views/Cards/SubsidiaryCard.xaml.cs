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
using Model;
using Laundry.Utils;
using Laundry.Utils.Controls;
using MongoDB.Driver;

namespace Laundry.Views.Cards
{
  /// <summary>
  /// Interaction logic for SubsidiaryCard.xaml
  /// </summary>
  public class SubsidiaryCardViewModel : Card<Subsidiary>
  {
    public EmployeeDataGridViewModel AdvisorsGrid { get; }

    public override Subsidiary Entity
    {
      get { return base.Entity; }

      set
      {
        base.Entity = value;
        this.AdvisorsGrid.Filter = Builders<Employee>.Filter.And(
          Builders<Employee>.Filter.Eq(nameof(Employee.Subsidiary), value.Id),
          Builders<Employee>.Filter.Eq(nameof(Employee.Profession), EmployeeProfession.Advisor)
        );
        this.AdvisorsGrid.Refresh(0, int.MaxValue);
      }
    }

    public SubsidiaryCardViewModel(IEventAggregator eventAggregator, EmployeeDataGridViewModel advisorsGrid) : base(
      eventAggregator, Screens.SubsidiaryEditor)
    {
      this.AdvisorsGrid = advisorsGrid;
      this.AdvisorsGrid.IsCompact = true;
      this.AdvisorsGrid.DisplaySelectionColumn = false;
    }
  }
}