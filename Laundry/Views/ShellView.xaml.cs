using System.Collections.Generic;
using Caliburn.Micro;
using Model;
using Laundry.Utils;
using Laundry.Views;
using MaterialDesignThemes.Wpf;
using PropertyChanged;

namespace Laundry
{
  /// <summary>
  /// Главное окно приложения
  /// </summary>
  public class ShellViewModel : Conductor<ActivityScreen>, IShell, IHandle<Screens>, IHandle<DrawerState>
  {
    public Visibilities Visibilities { get; set; }

    private readonly EmployeeCardViewModel _employeeCard;
    private IScreenFactory _factory;
    private ConnectionLostDialogViewModel _connectionLostDialog;
    private readonly IModel _model;
    private PaletteHelper _paletteHelper;

    public bool IsDrawerOpened { get; set; }

    [AlsoNotifyFor(nameof(CurrentName))]
    public Employee CurrentEmployee { get; set; }

    /// <summary>
    /// Запись текущего DialogHost в статический класс расширения хоста
    /// </summary>
    /// <param name="host">DialogHost</param>
    public void HostLoaded(DialogHost host)
    {
      DialogHostExtensions.Current = host;
    }

    /// <summary>
    /// ФИО текущего работника
    /// </summary>
    public string CurrentName
    {
      get
      {
        return $"{CurrentEmployee?.Surname ?? ""} {CurrentEmployee?.Name ?? ""} {CurrentEmployee?.Patronymic ?? ""}";
      }
    }

    public ShellViewModel(IEventAggregator eventAggregator, Visibilities visibilities,
      ConnectionLostDialogViewModel connectionLostDialog, IModel model, IScreenFactory factory)
    {
      Visibilities = visibilities;
      eventAggregator.Subscribe(this);

      //_employeeCard = employeeCard;
      this._factory = factory;
      this._connectionLostDialog = connectionLostDialog;
      _model = model;
      this.Handle(Screens.Login);
      _paletteHelper = new PaletteHelper();

      model.Connected += OnConnected;
      model.ConnectionLost += OnConnectionLost;
    }

    /// <summary>
    /// Установка цветовой темы работника при входе
    /// </summary>
    /// <param name="obj"></param>
    private void OnConnected(Employee obj)
    {
      this.CurrentEmployee = obj;
      this._paletteHelper.SetLightDark(obj.IsDarkTheme);
    }


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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="message"></param>
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


    public bool IsDarkTheme
    {
      get { return this.CurrentEmployee.IsDarkTheme; }
      set
      {
        this.CurrentEmployee.IsDarkTheme = value;
        this._model.Employees.UpdateTheme(CurrentEmployee, value);
        this._paletteHelper.SetLightDark(value);
      }
    }
  }
}