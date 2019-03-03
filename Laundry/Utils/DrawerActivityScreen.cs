﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using Laundry.Model;

namespace Laundry.Utils
{
  public class DrawerActivityScreen : ActivityScreen
  {
    private bool _isDrawerButtonChecked;

    public bool IsDrawerButtonChecked
    {
      get => _isDrawerButtonChecked;
      set
      {
        _isDrawerButtonChecked = value;
        this.EventAggregator.PublishOnUIThread(value ? DrawerState.Opened : DrawerState.Closed);
        _isDrawerButtonChecked = false;
        NotifyOfPropertyChange(nameof(IsDrawerButtonChecked));
      }
    }

    public DrawerActivityScreen(IEventAggregator aggregator, IModel model) : base(aggregator, model)
    {
    }
  }
}