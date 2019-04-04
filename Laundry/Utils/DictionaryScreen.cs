using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using Laundry.Model;
using Laundry.Model.CollectionRepositories;
using Laundry.Model.DatabaseClients;
using Laundry.Utils.Controls;

namespace Laundry.Utils
{
  public class DictionaryScreen<TEntity, TGrid> : DrawerActivityScreen
    where TEntity : IRepositoryElement
    where TGrid : IEntityGrid<TEntity, Repository<TEntity>>
  {
    public PaginatorViewModel Paginator { get; set; }
    public TGrid EntityGrid { get; set; }

    public DictionaryScreen(IEventAggregator aggregator, IModel model, PaginatorViewModel paginator, TGrid entityGrid) :
      base(aggregator, model)
    {
      this.EntityGrid = entityGrid;

      this.Paginator = paginator;
      this.Paginator.ElementsName = "Объектов";
      this.Paginator.RegisterPaginable(entityGrid);
    }

    protected override void OnActivate()
    {
      base.OnActivate();
    }

    public void AddEntity()
    {
      this.EntityGrid.Add();
    }
  }
}