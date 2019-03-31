﻿using System;
using System.Security;
using System.Windows.Controls;
using MaterialDesignThemes.Wpf;
using Caliburn.Micro;
using Laundry.Model;
using Laundry.Utils;

namespace Laundry.Views
{
  /// <summary>
  /// Interaction logic for LoginScreen.xaml
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
      catch (Exception e)
      {
        
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
