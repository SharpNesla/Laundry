using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Caliburn.Micro;
using Laundry.Model;
using Laundry.Model.CollectionRepositories;
using Laundry.Model.DatabaseClients;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Laundry.Utils.Controls.EntitySearchControls
{
  public abstract class EntitySearchBox<TEntity, TRepository> : PropertyChangedBase
    where TEntity : IRepositoryElement
    where TRepository : Repository<TEntity>
  {
    private TEntity _selectedEntity;
    protected readonly TRepository Repository;
    private string _entityText;
    public IList<TEntity> Entities { get; set; }

    public string EntityText
    {
      get
      {
        if (SelectedEntity != null)
        {
          _entityText = this.SelectedEntity.Signature;
          return this.SelectedEntity.Signature;

        }
        else
        {
          return "";
        }
      }
      set
      {
        _entityText = value;
        OnEntitySearch(_entityText);
      }
    }

    public TEntity SelectedEntity
    {
      get { return _selectedEntity; }
      set
      {
        _selectedEntity = value;
        ClientChanged?.Invoke(value);
      }
    }

    public event Action<TEntity> ClientChanged;
    public abstract void OnEntitySearch(string entityText);
    protected EntitySearchBox(TRepository repository)
    {
      this.Repository = repository;
      this.Entities = new List<TEntity>(Repository.Get(0, 10));
    }

    public void OnInputChanged(ComboBox box)
    {
      box.IsDropDownOpen = true;
    }
    
  }
}