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

    public TEntity Client { get; set; }

    public virtual string EditorTitle
    {
      get { return !IsNew ? $"Редактирование клиента №{Client.Id}" : "Редактирование нового клиента"; }
    }

    protected TRepository EntityRepository { get; set; }

    public EditorScreen(IEventAggregator aggregator, IModel model, TRepository entityRepo) : base(aggregator, model)
    {
      this.EventAggregator.Subscribe(this);
      this.EntityRepository = entityRepo;

      this.IsNew = true;
      this.Client = new TEntity();
    }

    public void Discard()
    {
      ChangeApplicationScreen(Screens.Context);
    }

    public virtual void Handle(TEntity message)
    {
      this.Client = this.EntityRepository.GetById(message.Id);
      this.IsNew = false;
      this.EventAggregator.Unsubscribe(this);
    }

    public void ApplyChanges()
    {
      if (IsNew)
      {
        EntityRepository.Add(this.Client);
      }
      else
      {
        EntityRepository.Update(this.Client);
      }

      ChangeApplicationScreen(Screens.Context);
    }
  }
}