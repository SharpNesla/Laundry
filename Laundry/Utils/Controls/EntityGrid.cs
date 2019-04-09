using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using Laundry.Model;
using Laundry.Model.CollectionRepositories;
using Laundry.Model.DatabaseClients;
using Laundry.Views;
using MaterialDesignThemes.Wpf;
using MongoDB.Driver;
using Action = System.Action;

namespace Laundry.Utils.Controls
{
  public interface IEntityGrid<out TEntity> : IPaginable where TEntity : IRepositoryElement
  {
    IReadOnlyList<TEntity> Entities { get; }
    TEntity SelectedEntity { get; }

    void Add();
    void Edit();
    void Remove();
    void Refresh();
  }

  public class EntityGrid<TEntity, TRepository, TCard> : PropertyChangedBase, IEntityGrid<TEntity>
    where TEntity : IRepositoryElement
    where TRepository : Repository<TEntity>
    where TCard : Card<TEntity>
  {
    private Card<TEntity> _card;
    private IEventAggregator _eventAggregator;
    private Screens _editScreen;
    public IReadOnlyList<TEntity> Entities { get; set; }
    public TEntity SelectedEntity { get; set; }
    public FilterDefinition<TEntity> Filter;
    private readonly DeleteDialogViewModel _shure;
    public TRepository Repo { get; }

    public bool DisplaySelectionColumn { get; set; }

    public event Action<TEntity> RemoveButtonClick;

    public EntityGrid(IEventAggregator eventAggregator, TCard card, TRepository repo,
      DeleteDialogViewModel shure, Screens editScreen, bool displaySelectColumn = true)
    {
      this._card = card;
      this._editScreen = editScreen;
      this._eventAggregator = eventAggregator;
      this.Repo = repo;
      this._shure = shure;
      this.DisplaySelectionColumn = displaySelectColumn;
    }

    public virtual long Count
    {
      get { return this.Repo.GetCount(); }
    }

    public virtual void Refresh(int page, int elements)
    {
      var repo =
        (Filter == null ? Repo.Get(page * elements, elements) : Repo.Get(page * elements, elements, Filter)) as
        List<TEntity>;
      this.Entities = repo.AsReadOnly();
    }

    public event Action StateChanged;

    public void ShowInfoCard()
    {
      if (SelectedEntity != null)
      {
        _card.Entity = SelectedEntity;

        DialogHostExtensions.ShowCaliburnVM(_card);
      }
    }

    public void ShowInfoCard(TEntity context)
    {
      if (context != null)
      {
        _card.Entity = context;

        DialogHostExtensions.ShowCaliburnVM(_card);
      }
    }

    public void Add()
    {
      _eventAggregator.PublishOnUIThread(_editScreen);
      StateChanged?.Invoke();
    }

    public void Edit()
    {
      _eventAggregator.PublishOnUIThread(_editScreen);
      _eventAggregator.PublishOnUIThread(SelectedEntity);
      StateChanged?.Invoke();
    }

    public async void Remove()
    {
      var isDelete = await _shure.AskQuestion();
      if (isDelete)
      {
        this.Repo.Remove(SelectedEntity);
        StateChanged?.Invoke();
      }
    }
  }
}