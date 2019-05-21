using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Model;

namespace Laundry.Views
{
  /// <summary>
  /// Класс для работы с отсечением областей видимости,
  /// принимает модель, как зависимость и читает из неё профессию текущего пользователя
  /// </summary>
  public class Visibilities : Caliburn.Micro.PropertyChangedBase
  {
    private readonly IModel _model;

    public bool Courier => _model.CurrentUser?.Profession == EmployeeProfession.Courier;
    public bool Advisor => _model.CurrentUser?.Profession == EmployeeProfession.Advisor;
    public bool Driver => _model.CurrentUser?.Profession == EmployeeProfession.Driver;
    public bool Washer => _model.CurrentUser?.Profession == EmployeeProfession.Washer;
    public bool Director => _model.CurrentUser?.Profession == EmployeeProfession.Director;

    public bool DirectorAdvisorWasher =>
      _model.CurrentUser?.Profession == EmployeeProfession.Director ||
      _model.CurrentUser?.Profession == EmployeeProfession.Advisor ||
      _model.CurrentUser?.Profession == EmployeeProfession.Washer;

    public bool DirectorAdvisor =>
      _model.CurrentUser.Profession == EmployeeProfession.Director ||
      _model.CurrentUser.Profession == EmployeeProfession.Advisor;

    public Visibilities(IModel model)
    {
      _model = model;
      _model.Connected += OnModelOnConnected;
    }

    /// <summary>
    /// Обработчик подключения к модели, необходим для
    /// сигнализации изменения visibility всего приложения
    /// при входе нового пользователя
    /// </summary>
    /// <param name="x"></param>
    private void OnModelOnConnected(Employee x)
    {
      this.NotifyOfPropertyChange(string.Empty);
    }
  }
}