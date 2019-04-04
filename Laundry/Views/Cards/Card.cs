using Laundry.Model.DatabaseClients;

namespace Laundry.Views
{
  public class Card<TEntity> where TEntity : IRepositoryElement
  {
    public TEntity Entity { get; set; }
    public Card(TEntity entity)
    {
      this.Entity = entity;
    }
    
  }
}