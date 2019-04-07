using Caliburn.Micro;
using Laundry.Model.DatabaseClients;
using Laundry.Utils;

namespace Laundry.Views
{
  public class Card<TEntity> : PropertyChangedBase where TEntity : IRepositoryElement
  {
    private Screens _editorScreen;
    private readonly IEventAggregator _eventAggregator;
    public virtual TEntity Entity { get; set; }
    public Card(IEventAggregator eventAggregator,  Screens editorScreen)
    {
      this._eventAggregator = eventAggregator;
      this._editorScreen = editorScreen;
    }

    public void Edit()
    {
      _eventAggregator.PublishOnUIThread(Screens.ClientEditor);
      _eventAggregator.PublishOnUIThread(this.Entity);
    }
  }
}