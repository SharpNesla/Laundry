using Laundry.Views;
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
namespace Laundry
{
  /// <summary>
  /// Логика взаимодействия для ShellView.xaml
  /// </summary>
  public class ShellViewModel : Conductor<object>, IShell, IHandle<Utils.Screens>, IHandle<Utils.DrawerState>
  {

    
    public bool IsDrawerOpened
    {
      get => _isDrawerOpened;
      set
      {
        _isDrawerOpened = value;
        NotifyOfPropertyChange(nameof(IsDrawerOpened));
      }
    }

    private bool _isDrawerOpened;

    private readonly Dictionary<Utils.Screens, PropertyChangedBase> _screens;

    public ShellViewModel(IEventAggregator eventAggregator, IModel model, LoginScreenViewModel screen, DashBoardViewModel dashboard)
    {
      this._screens = new Dictionary<Utils.Screens, PropertyChangedBase>
      {
        [Utils.Screens.DashBoard] = dashboard,
        [Utils.Screens.Login] = screen
      };
      
      eventAggregator.Subscribe(this);
      this.Handle(Utils.Screens.Login);
    }

    public void Handle(Utils.Screens message)
    {
      this.ActivateItem(this._screens[message]);
    }

    public void SetScreen(Screens message)
    {
      this.Handle(message);
      this.IsDrawerOpened = false;
    }

    public void Handle(DrawerState message)
    {
      this.IsDrawerOpened = message == DrawerState.Opened;
    }
  }
}


