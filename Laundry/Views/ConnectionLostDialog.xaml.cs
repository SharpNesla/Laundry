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
using Laundry.Utils;

namespace Laundry.Views
{
  /// <summary>
  /// Окно потери соединения
  /// </summary>
  public class ConnectionLostDialogViewModel : Screen
  {
    private readonly EventAggregator _aggregator;

    public ConnectionLostDialogViewModel(EventAggregator aggregator)
    {
      _aggregator = aggregator;
    }

    public void Accept()
    {
      this._aggregator.PublishOnUIThread(Screens.Login);
    }
  }
}
