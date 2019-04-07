using Caliburn.Micro;
using Laundry.Model;
using Laundry.Model.CollectionRepositories;
using Laundry.Model.DatabaseClients;
using PropertyChanged;

namespace Laundry.Utils
{
  public class EditorScreen<TRepository, TEntity> : ActivityScreen, IHandle<TEntity>
    where TEntity : IRepositoryElement, new()
    where TRepository : Repository<TEntity>
  {
    [AlsoNotifyFor(nameof(EditorTitle))]
    public bool IsNew { get; set; }

    public TEntity Entity { get; set; }

    public virtual string EditorTitle
    {
      get { return !IsNew ? $"Редактирование {_clientName} №{Entity.Id}" : $"Редактирование нового {_clientName}"; }
    }

    private readonly string _clientName;

    protected TRepository EntityRepository { get; set; }

    public EditorScreen(IEventAggregator aggregator, IModel model, TRepository entityRepo, string entityTitleName = "объекта") : base(aggregator, model)
    {
      this.EventAggregator.Subscribe(this);
      this.EntityRepository = entityRepo;

      this._clientName = entityTitleName;

      this.IsNew = true;
      this.Entity = new TEntity();
    }

    public void Discard()
    {
      ChangeApplicationScreen(Screens.Context);
    }

    public virtual void Handle(TEntity message)
    {
      this.Entity = this.EntityRepository.GetById(message.Id);
      this.IsNew = false;
      this.EventAggregator.Unsubscribe(this);
    }

    public void ApplyChanges()
    {
      if (IsNew)
      {
        EntityRepository.Add(this.Entity);
      }
      else
      {
        EntityRepository.Update(this.Entity);
      }

      ChangeApplicationScreen(Screens.Context);
    }
  }
}