using System.Collections.Generic;
using Caliburn.Micro;
using Laundry.Model;
using Laundry.Utils;
using Laundry.Views;

namespace Laundry
{
  /// <summary>
  /// Логика взаимодействия для ShellView.xaml
  /// </summary>
  public class ShellViewModel : Conductor<ActivityScreen>, IShell, IHandle<Screens>, IHandle<DrawerState>
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

    private readonly Dictionary<Screens, ActivityScreen> _screens;

    public ShellViewModel(IEventAggregator eventAggregator, IModel model,
      LoginScreenViewModel screen, ClientDictionaryViewModel clientDictionary, DashBoardViewModel dashboard)
    {
      this._screens = new Dictionary<Screens, ActivityScreen>
      {
        [Screens.DashBoard] = dashboard,
        [Screens.Login] = screen,
        [Screens.ClientDictionary] = clientDictionary
      };
      
      eventAggregator.Subscribe(this);
      this.Handle(Screens.Login);
    }

    public void Handle(Screens message)
    {
      this._screens[message].Context = ActiveItem;
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


