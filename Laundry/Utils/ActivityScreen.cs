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
