using System;
using System.Collections.Generic;
using System.ComponentModel;
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
  public class EntitySearchBox<TEntity, TRepository> : PropertyChangedBase, IDataErrorInfo
    where TEntity : IRepositoryElement
    where TRepository : Repository<TEntity>
  {
    private TEntity _selectedEntity;
    protected readonly TRepository Repository;
    private readonly bool _isRequired;
    private string _entityText;
    private FilterDefinition<TEntity> _filter;

    public FilterDefinition<TEntity> Filter
    {
      get { return _filter; }
      set
      {
        _filter = value;

        this.Entities = new List<TEntity>(Repository.Get(0, 10, value));
      }
    }

    public IList<TEntity> Entities { get; set; }

    public string Label { get; set; }

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
          return _entityText;
        }
      }
      set
      {
        _entityText = value;

        if (value == String.Empty)
        {
          this.Entities = new List<TEntity>(Repository.Get(0, 10, Filter));
        }
        else
        {
          OnEntitySearch(_entityText);
        }
      }
    }

    public TEntity SelectedEntity
    {
      get { return _selectedEntity; }
      set
      {
        _selectedEntity = value;
        EntityChanged?.Invoke(value);
      }
    }

    public event Action<TEntity> EntityChanged;

    public void OnEntitySearch(string entityText)
    {
      this.Entities = (IList<TEntity>) Repository.GetBySearchString(entityText, Filter);
    }

    protected EntitySearchBox(TRepository repository, string label = "Объект", FilterDefinition<TEntity> filter = null, bool isRequired = true)
    {
      this.Repository = repository;
      _isRequired = isRequired;
      this.Label = label;
      this.Filter = filter;
      this.Entities = new List<TEntity>(Repository.Get(0, 10, Filter));
    }

    public void OnInputChanged(ComboBox box)
    {
      box.IsDropDownOpen = true;
    }

    public string this[string columnName]
    {
      get
      {
        var errorString = "";
        if (columnName == nameof(this.SelectedEntity) && _isRequired)
        {
          if (this.SelectedEntity == null)
          {
            errorString = "Работник не может быть пустым";
          }
        }

        return errorString;
      }
    }

    public string Error { get; }
  }
}