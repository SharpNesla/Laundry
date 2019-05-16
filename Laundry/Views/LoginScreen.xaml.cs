using System;
using System.Collections.Generic;
using System.Security;
using System.Threading.Tasks;
using System.Windows.Controls;
using MaterialDesignThemes.Wpf;
using Caliburn.Micro;
using Model;
using Laundry.Utils;

namespace Laundry.Views
{
  /// <summary>
  /// Окно входа в систему
  /// </summary>
  public class LoginScreenViewModel : ActivityScreen
  {
    public string Password { get; set; }
    public string Username { get; set; }

    public LoginScreenViewModel(IEventAggregator aggregator, IModel model) : base(aggregator, model)
    {
    }

    public void Login()
    {
      try
      {
        this.Model.Connect(Username, Password);
        ChangeApplicationScreen(Screens.DashBoard);
      }
      catch (UnauthorizedAccessException e)
      {
        var messageQueue = SnackBar.MessageQueue;
        var message = "Неправильное имя пользователя или пароль";
      
        messageQueue.Enqueue(message);
      }
      
    }

    public void Settings()
    {
      DialogHost.Show(new LoginSettings());
    }

    public void SnackBarLoaded(Snackbar bar)
    {
      this.SnackBar = bar;
    }

    public Snackbar SnackBar { get; set; }

    public void PasswordChanged(PasswordBox box)
    {
      Password = box.Password;
    }
  }
}
