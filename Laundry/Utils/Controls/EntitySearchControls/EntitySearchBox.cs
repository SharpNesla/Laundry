using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Caliburn.Micro;
using Model;
using Model.CollectionRepositories;
using Model.DatabaseClients;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Laundry.Utils.Controls.EntitySearchControls
{
  /// <summary>
  /// Базовый класс для динамических 
  /// полей со списком поисковых строк
  /// </summary>
  /// <typeparam name="TEntity">Сущность, по которой ведётся поиск</typeparam>
  /// <typeparam name="TRepository">Репозиторий сущности</typeparam>
  public abstract class EntitySearchBox<TEntity, TRepository> : PropertyChangedBase, IDataErrorInfo
    where TEntity : RepositoryElement
    where TRepository : Repository<TEntity>
  {
    private TEntity _selectedEntity;
    protected readonly TRepository Repository;
    private readonly bool _isRequired;
    private string _entityText;
    private FilterDefinition<TEntity> _filter;

    /// <summary>
    /// Фильтр с учётом которого ведётся поиск
    /// </summary>
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

    /// <summary>
    /// Текст в подсказке к полю
    /// </summary>
    public string Label { get; set; }


    /// <summary>
    /// Строка поиска
    /// в геттере проверка на null,
    /// в сеттере при изменении строки триггерится поиск
    /// </summary>
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

        if (string.IsNullOrEmpty(value))
        {
          this.Entities = new List<TEntity>(Repository.Get(0, 10, Filter));
        }
        else
        {
          OnEntitySearch(_entityText);
        }
      }
    }

    /// <summary>
    /// Выбранная сущность, при смене триггерит событие EntityChanged
    /// </summary>
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
      this.Entities = Repository.GetBySearchString(entityText, Filter) as IList<TEntity>;
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

    #region Реализация IDataErrorInfo для валидации

    public string this[string columnName]
    {
      get
      {
        var errorString = "";
        if (columnName == nameof(this.SelectedEntity) && _isRequired)
        {
          if (this.SelectedEntity == null)
          {
            errorString = $"{this.Label} не может быть пустым";
          }
        }

        return errorString;
      }
    }

    public string Error { get; }

    #endregion
  }
}