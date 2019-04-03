using System.Collections.Generic;
using Caliburn.Micro;
using Laundry.Model;
using Laundry.Utils;
using Laundry.Views;
using PropertyChanged;

namespace Laundry
{
  /// <summary>
  /// Логика взаимодействия для ShellView.xaml
  /// </summary>

  public class ShellViewModel : Conductor<ActivityScreen>, IShell, IHandle<Screens>, IHandle<DrawerState>
  {
    private IScreenFactory _factory;

    public bool IsDrawerOpened { get; set; }

    [AlsoNotifyFor(nameof(CurrentName))]
    public Employee CurrentEmployee { get; set; }

    public string CurrentName
    {
      get { return $"{CurrentEmployee?.Surname ?? ""} {CurrentEmployee?.Name ?? ""} {CurrentEmployee?.Patronymic ?? ""}"; }
    }

    public ShellViewModel(IEventAggregator eventAggregator, IModel model, IScreenFactory factory)
    {
      eventAggregator.Subscribe(this);
      
      this._factory = factory;
      this.Handle(Screens.Login);
      this.CurrentUser = model.CurrentUser;
      model.Connected += OnConnected;
    }

    private void OnConnected(Employee obj)
    {
      this.CurrentEmployee = obj;
    }


    public Employee CurrentUser { get; set; }


    public void SetScreen(Screens message)
    {
      this.Handle(message);
      this.IsDrawerOpened = false;
    }

    public void Handle(DrawerState message)
    {
      this.IsDrawerOpened = message == DrawerState.Opened;
    }

    public void Handle(Screens message)
    {
      if (message == Screens.Context)
      {
        this.ActivateItem(this.ActiveItem.Context);
      }
      else
      {
        var screen = _factory.GetScreen(message);
        screen.Context = ActiveItem;
        this.ActivateItem(screen);
      }
      
    }
  }
}


