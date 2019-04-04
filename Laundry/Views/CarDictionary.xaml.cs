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

namespace Laundry.Views
{
  /// <summary>
  /// Interaction logic for CarDictionary.xaml
  /// </summary>
  public class CarDictionaryViewModel : DrawerActivityScreen
  {
    public IList<Car> Cars{ get; set; }
    public Car SelectedCar { get; set; }

    public CarDictionaryViewModel(IEventAggregator aggregator, IModel model, ClientCardViewModel cardViewModel,
      PaginatorViewModel paginator) : base(aggregator,
      model)
    {

      this.Paginator = paginator;
      this.Paginator.ElementsName = "Автомобилей";

      this.Paginator.Changed += RefreshCars;
    }

    private void RefreshCars(int page, int elements)
    {
      this.Cars = Model.Cars.Get(page * elements, elements);

    }

    public PaginatorViewModel Paginator { get; set; }

    protected override void OnActivate()
    {
      base.OnActivate();
      Paginator.Count = Model.Clients.GetCount();
      RefreshCars(this.Paginator.CurrentPage - 1, this.Paginator.ElementsPerPage);
    }

    public void RemoveCar()
    {
      Model.Cars.Remove(SelectedCar);
      this.Paginator.Count = Model.Cars.GetCount();
      RefreshCars(this.Paginator.CurrentPage - 1, this.Paginator.ElementsPerPage);
    }

    public void EditCar()
    {
      ChangeApplicationScreen(Screens.CarEditor);
      EventAggregator.PublishOnUIThread(this.SelectedCar);
    }

    public void AddCar()
    {
      ChangeApplicationScreen(Screens.CarEditor);
    }
  }
}