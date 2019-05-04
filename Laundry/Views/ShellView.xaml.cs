using System.Collections.Generic;
using Caliburn.Micro;
using Laundry.Model;
using Laundry.Utils;
using Laundry.Views;
using MaterialDesignThemes.Wpf;
using PropertyChanged;

namespace Laundry
{
  /// <summary>
  /// Логика взаимодействия для ShellView.xaml
  /// </summary>

  public class ShellViewModel : Conductor<ActivityScreen>, IShell, IHandle<Screens>, IHandle<DrawerState>
  {
    private readonly EmployeeCardViewModel _employeeCard;
    private IScreenFactory _factory;
    private ConnectionLostDialogViewModel _connectionLostDialog;

    public bool IsDrawerOpened { get; set; }

    [AlsoNotifyFor(nameof(CurrentName))]
    public Employee CurrentEmployee { get; set; }

    public void HostLoaded(DialogHost host)
    {
      DialogHostExtensions.Current = host;
    }

    public string CurrentName
    {
      get { return $"{CurrentEmployee?.Surname ?? ""} {CurrentEmployee?.Name ?? ""} {CurrentEmployee?.Patronymic ?? ""}"; }
    }

    public ShellViewModel(IEventAggregator eventAggregator,ConnectionLostDialogViewModel connectionLostDialog, IModel model, IScreenFactory factory)
    {
      eventAggregator.Subscribe(this);

      //_employeeCard = employeeCard;
      this._factory = factory;
      this._connectionLostDialog = connectionLostDialog;
      this.Handle(Screens.Login);
      this.CurrentUser = model.CurrentUser;
      model.Connected += OnConnected;
      model.ConnectionLost += OnConnectionLost;
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

    public async void ShowSelfUserCard()
    {
      _employeeCard.Entity = CurrentEmployee;
      await DialogHostExtensions.ShowCaliburnVM(this._employeeCard);
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

    public async void OnConnectionLost()
    {
      await DialogHostExtensions.ShowCaliburnVM(_connectionLostDialog);
      this.Handle(Screens.Login);
    }
  }
}


