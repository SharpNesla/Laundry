using Caliburn.Micro;
using Model.DatabaseClients;
using Laundry.Utils;

namespace Laundry.Views
{
  public class Card<TEntity> : Screen where TEntity : IRepositoryElement
  {
    public Visibilities Visibilities { get; }
    private Screens _editorScreen;
    protected IEventAggregator _eventAggregator;
    public virtual TEntity Entity { get;
      set; }
    public Card(IEventAggregator eventAggregator,  Screens editorScreen, Visibilities visibilities = null)
    {
      Visibilities = visibilities;
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