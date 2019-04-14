﻿using System;
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
  public class EntitySearchBox<TEntity, TRepository> : PropertyChangedBase
    where TEntity : IRepositoryElement
    where TRepository : Repository<TEntity>
  {
    private TEntity _selectedEntity;
    protected readonly TRepository Repository;
    private string _entityText;

    public FilterDefinition<TEntity> Filter { get; set; }
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
    protected EntitySearchBox(TRepository repository, string label = "Объект", FilterDefinition<TEntity> filter = null)
    {
      this.Repository = repository;
      this.Label = label;
      this.Filter = filter;
      this.Entities = new List<TEntity>(Repository.Get(0, 10, Filter));
    }

    public void OnInputChanged(ComboBox box)
    {
      box.IsDropDownOpen = true;
    }
    
  }
}