using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Caliburn.Micro;
using Laundry.Model;

namespace Laundry.Utils
{
  public abstract class ActivityScreen : Screen
  {
    public IEventAggregator EventAggregator { get; private set; }
    public IModel Model { get; private set; }

    public ActivityScreen Context { get; set; }  

    public Visibility Courier => this.Model.CurrentUser.Profession == EmployeeProfession.Courier ? Visibility.Visible : Visibility.Hidden;
    public Visibility Director => this.Model.CurrentUser.Profession == EmployeeProfession.Director ? Visibility.Visible : Visibility.Hidden;
    public Visibility Washer => this.Model.CurrentUser.Profession == EmployeeProfession.Washer ? Visibility.Visible : Visibility.Hidden;
    public Visibility Advisor => this.Model.CurrentUser.Profession == EmployeeProfession.Advisor ? Visibility.Visible : Visibility.Hidden;

    protected ActivityScreen(IEventAggregator aggregator, IModel model)
    {
      this.EventAggregator = aggregator;
      this.Model = model;
    }

    public void ChangeApplicationScreen(Screens screen)
    {
      this.EventAggregator.PublishOnUIThread(screen);
    }

    public void ShowActivityMenu(Button button)
    {
      if (button.ContextMenu != null) button.ContextMenu.IsOpen = true;
    }

  }
}
