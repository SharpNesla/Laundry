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
using Laundry.Model.CollectionRepositories;
using Laundry.Views.Cards;

namespace Laundry.Utils.Controls
{
  /// <summary>
  /// Interaction logic for CarDataGrid.xaml
  /// </summary>
  public class CarDataGridViewModel : EntityGrid<Car, Repository<Car>, CarCardViewModel>
  {
    public CarDataGridViewModel(IEventAggregator eventAggregator, CarCardViewModel card, IModel model) 
      : base(eventAggregator, card, model.Cars, Screens.CarEditor)
    {
    }
  }
}
