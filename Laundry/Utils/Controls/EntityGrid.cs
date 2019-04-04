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
using Action = System.Action;

namespace Laundry.Utils.Controls
{
  public interface IEntityGrid<TEntity, out TRepository> : IPaginable where TEntity : IRepositoryElement where TRepository : Repository<TEntity>
  {
    IList<TEntity> Entities { get; set; }
    TEntity SelectedEntity { get; set; }
    TRepository Repo { get; }

    void ShowInfoCard(TEntity context);
    void Add();
    void Edit();
    void Remove();
    void Refresh();
  }

  public class EntityGrid<TEntity, TRepository> : PropertyChangedBase, IEntityGrid<TEntity, TRepository> where TEntity : IRepositoryElement
    where TRepository : Repository<TEntity>
  {
    private Card<TEntity> _card;
    private IEventAggregator _eventAggregator;
    private Screens _editScreen;
    public IList<TEntity> Entities { get; set; }
    public TEntity SelectedEntity { get; set; }

    public TRepository Repo { get; }

    public event Action<TEntity> RemoveButtonClick;

    public EntityGrid(IEventAggregator eventAggregator, Card<TEntity> card, TRepository repo , Screens editScreen)
    {
      this._card = card;
      this._editScreen = editScreen;
      this._eventAggregator = eventAggregator;
      this.Repo = repo;
    }

    public long Count
    {
      get { return this.Repo.GetCount(); }
    }

    public virtual void Refresh(int page, int elements)
    {
      this.Entities = Repo.Get(page * elements, elements);
    }

    public event Action StateChanged;

    public void ShowInfoCard(TEntity context)
    {
      if (context != null)
      {
        //Ищем View для ViewModel карточки клиента (Caliburn)
        var view = ViewLocator.LocateForModel(_card, null, null);
        ViewModelBinder.Bind(this._card, view, null);

        this._eventAggregator.PublishOnUIThread(context);

        DialogHost.Show(view);
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

    public void Remove()
    {
      this.Repo.Remove(SelectedEntity);
      StateChanged?.Invoke();
    }
  }
}
