using Caliburn.Micro;
using Model.DatabaseClients;
using Laundry.Utils;

namespace Laundry.Views
{

  /// <summary>
  /// Базовый класс для карточки сущности
  /// </summary>
  /// <typeparam name="TEntity">Отображаемая сущность</typeparam>
  public class Card<TEntity> : Screen where TEntity : RepositoryElement
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

    /// <summary>
    /// Обработчик кнопки "редактировать" в карточке
    /// </summary>
    public virtual void Edit()
    {
      _eventAggregator.PublishOnUIThread(_editorScreen);
      _eventAggregator.PublishOnUIThread(this.Entity);
    }
  }
}