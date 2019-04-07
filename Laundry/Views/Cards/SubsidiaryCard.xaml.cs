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

namespace Laundry.Views.Cards
{
  /// <summary>
  /// Interaction logic for SubsidiaryCard.xaml
  /// </summary>
  public class SubsidiaryCardViewModel : Card<Subsidiary>
  {
    public SubsidiaryCardViewModel(IEventAggregator eventAggregator) : base(eventAggregator, Screens.SubsidiaryEditor)
    {
    }
  }
}
