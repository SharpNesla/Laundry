﻿using Caliburn.Micro;
using Laundry.Model.DatabaseClients;
using Laundry.Utils;

namespace Laundry.Views
{
  public class Card<TEntity> : PropertyChangedBase where TEntity : IRepositoryElement
  {
    private Screens _editorScreen;
    protected IEventAggregator _eventAggregator;
    public virtual TEntity Entity { get; set; }
    public Card(IEventAggregator eventAggregator,  Screens editorScreen)
    {
      this._eventAggregator = eventAggregator;
      this._editorScreen = editorScreen;
    }

    public virtual void Edit()
    {
      _eventAggregator.PublishOnUIThread(_editorScreen);
      _eventAggregator.PublishOnUIThread(this.Entity);
    }
  }
}