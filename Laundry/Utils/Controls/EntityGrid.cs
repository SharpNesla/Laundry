using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Caliburn.Micro;
using Model;
using Model.CollectionRepositories;
using Model.DatabaseClients;
using Laundry.Utils.Converters;
using Laundry.Views;
using MaterialDesignThemes.Wpf;
using MongoDB.Driver;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using Action = System.Action;
using NPOI.XSSF.UserModel;

namespace Laundry.Utils.Controls
{
  /// <summary>
  /// Интерфейс таблицы с сущностями
  /// Является пагинируемой (наследует IPaginable)
  /// Ковариантен
  /// </summary>
  /// <typeparam name="TEntity">Тип сущности</typeparam>
  public interface IEntityGrid<out TEntity> : IPaginable where TEntity : RepositoryElement
  {
    IReadOnlyList<TEntity> Entities { get; }
    
    TEntity SelectedEntity { get; }

    void ExportToCSV();
    void ExportToExcel();
    
    /// <summary>
    /// Добавить элемент
    /// </summary>
    void Add();

    /// <summary>
    /// Редактирование выделенного элемента
    /// </summary>
    void Edit();

    /// <summary>
    /// Удаление выделенного элемента
    /// </summary>
    void Remove();

    /// <summary>
    /// Удаление выделенную группу элементов
    /// </summary>
    void RemoveSelectedGroup();
   
  }

  /// <inheritdoc cref="IEntityGrid{TEntity}" />
  /// <summary>
  /// Базовый класс таблицы с сущностями
  /// </summary>
  /// <typeparam name="TEntity">Тип сущности</typeparam>
  /// <typeparam name="TRepository">Тип repository для работы с данной сущностью</typeparam>
  /// <typeparam name="TCard">Тип карточки, показывающей информацию о данной сущности</typeparam>
  public abstract class EntityGrid<TEntity, TRepository, TCard> : PropertyChangedBase, IEntityGrid<TEntity>
    where TEntity : RepositoryElement
    where TRepository : Repository<TEntity>
    where TCard : Card<TEntity>
  {
    private Card<TEntity> _card;
    protected readonly IEventAggregator EventAggregator;
    private Screens _editScreen;


    public virtual IReadOnlyList<TEntity> Entities { get; set; }
    public TEntity SelectedEntity { get; set; }

    /// <summary>
    /// Компактный режим отображения таблицы (в карточках, действиях и.т.п.)
    /// </summary>
    public bool IsCompact { get; set; }

    /// <summary>
    /// Список выделенных сущностей
    /// </summary>
    public IReadOnlyList<TEntity> SelectedEntities
    {
      get { return this.Entities.Where(x => x.IsSelected).ToList(); }
    }
    /// <summary>
    /// Переопределяемый в наследниках конечный фильтр
    /// Используется для реализации панели филтров в интерфейсе
    /// </summary>
    public virtual FilterDefinition<TEntity> Filter
    {
      get { return BaseFilter; }
      set { BaseFilter = value; }
    }

    protected readonly DeleteDialogViewModel RemoveDialog;

    /// <summary>
    /// Базовый фильтр для таблицы, необходим для реализации специальных версий таблиц
    /// Примеры: таблица заказов для определённого клиента, таблица работников филиала и.т.п.
    /// </summary>
    protected FilterDefinition<TEntity> BaseFilter { get; private set; }

    public TRepository Repo { get; }
    public bool IsDisplaySubtotals { get; set; }
    public Visibilities Visibilities { get; }

    public virtual string[] TableSheetHeader => new[] {"№"};
    public virtual string TableSheetName => "Объекты";

    /// <summary>
    /// Св-во, к которому привязано состояние панели
    /// фильтров (скрыта или показана)
    /// </summary>
    public bool IsSearchDrawerOpened { get; set; }

    public bool DisplaySelectionColumn { get; set; }
    
    public string SearchString { get; set; }

    /// <summary>
    /// Конструктор, принимающий зависимости и параметры,
    /// определяющее поведение таблицы (часто задаются в наследниках)
    /// </summary>
    /// <param name="eventAggregator"></param>
    /// <param name="card"></param>
    /// <param name="repo">Reposutory сущности</param>
    /// <param name="removeDialog"></param>
    /// <param name="editScreen">Значение перечисления экрана-редактора сущности</param>
    /// <param name="visibilities"></param>
    /// <param name="entityName">Название сущности (используется для экспорта в таблицу)</param>
    /// <param name="displaySelectColumn">Показывать столбец выбора с флажками</param>
    protected EntityGrid(IEventAggregator eventAggregator, TCard card, TRepository repo,
      DeleteDialogViewModel removeDialog, Screens editScreen, Visibilities visibilities = null,
      string entityName = "объекта",
      bool displaySelectColumn = true)
    {
      this._card = card;
      this._editScreen = editScreen;
      this.EventAggregator = eventAggregator;
      this.Repo = repo;
      this.EntityName = entityName;
      this.RemoveDialog = removeDialog;
      this.DisplaySelectionColumn = displaySelectColumn;
      this.BaseFilter = Builders<TEntity>.Filter.Empty;
      this.Visibilities = visibilities;
    }

    public virtual long Count
    {
      get
      {
        if (!string.IsNullOrEmpty(SearchString))
        {
          return Repo.GetSearchStringCount(SearchString, Filter);
        }
        else
        {
          return this.Repo.GetCount(Filter);
        }
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// Обновить элементы таблицы согласно текущей странице
    /// </summary>
    /// <param name="page">Текущая страница</param>
    /// <param name="elements">Элементов на странице</param>
    public virtual void Refresh(int page, int elements)
    {
      if (!string.IsNullOrEmpty(SearchString))
      {
        this.Entities = Repo.GetBySearchString(SearchString, Filter, page * elements, elements);
      }
      else
      {
        this.Entities = Repo.Get(page * elements, elements, Filter);
      }
    }

    public event Action StateChanged;

    /// <summary>
    /// Показать карточку для выбранного в таблице объекта
    /// </summary>
    public async void ShowInfoCard()
    {
      if (SelectedEntity != null)
      {
        _card.Entity = SelectedEntity;

        await DialogHostExtensions.ShowCaliburnVM(_card);
      }
    }

    /// <summary>
    /// Показать карточку для сущности
    /// </summary>
    /// <param name="context">сущность</param>
    public async void ShowInfoCard(TEntity context)
    {
      if (context != null)
      {
        _card.Entity = context;

        await DialogHostExtensions.ShowCaliburnVM(_card);
      }
    }


    /// <inheritdoc />
    public virtual void Add()
    {
      EventAggregator.PublishOnUIThread(_editScreen);
      StateChanged?.Invoke();
    }


    /// <inheritdoc />
    public virtual void Edit()
    {
      EventAggregator.PublishOnUIThread(_editScreen);
      EventAggregator.PublishOnUIThread(SelectedEntity);
    }

    
    public virtual async void Remove()
    {
      var isDelete = await RemoveDialog.AskQuestion();
      if (isDelete)
      {
        this.Repo.Remove(SelectedEntity);
        StateChanged?.Invoke();
      }
    }

    public void RemoveSelectedGroup()
    {
      foreach (var selectedEntity in SelectedEntities)
      {
        this.Repo.Remove(selectedEntity);
      }

      RaiseStateChanged();
    }

    /// <summary>
    /// Подготовка XSSF книги excel (xlsx)
    /// Заполнение, расставление автофильтров, автоподстройка ширины
    /// </summary>
    /// <param name="workbook"></param>
    /// <returns></returns>
    protected virtual XSSFWorkbook PrepareWorkBook(XSSFWorkbook workbook)
    {
      var sheet = workbook.CreateSheet();

      var entities = this.Repo.Get(0, int.MaxValue, Filter);

      workbook.SetSheetName(0, this.TableSheetName);

      var header = sheet.CreateRow(0);

      for (var i = 0; i < this.TableSheetHeader.Length; i++)
      {
        header.CreateCell(i).SetCellValue(this.TableSheetHeader[i]);
      }

      foreach (var entity in entities)
      {
        PrepareEntityRow(sheet, entity);
      }

      for (var i = 0; i < this.TableSheetHeader.Length; i++)
      {
        sheet.AutoSizeColumn(i);
      }

      sheet.SetAutoFilter(new CellRangeAddress(0, 0, 0, TableSheetHeader.Length - 1));

      return workbook;
    }

    public void ExportToCSV()
    {
      var builder = MakeCSVStringFromXSSF();
      
      var dialog = new SaveFileDialog
      {
        InitialDirectory = @"~/Documents",
        Title = $"Путь к экспортируемой таблице {EntityName}",
        AddExtension = true,
        Filter = "Файлы CSV (*.csv)|*.csv|Все остальные файлы (*.*)|*.*"
      };
      if (dialog.ShowDialog() == DialogResult.OK)
      {
        if (!File.Exists(dialog.FileName))
        {
          File.Delete(dialog.FileName);
        }

        File.WriteAllText(dialog.FileName, builder, Encoding.UTF8);
      }
    }

    private string MakeCSVStringFromXSSF()
    {
      var builder = new StringBuilder();

      var entities = this.Repo.Get(0, int.MaxValue, Filter);
      var workbook = new XSSFWorkbook();
      var sheet = workbook.CreateSheet();

      foreach (var header in this.TableSheetHeader)
      {
        builder.Append($"{header};");
      }

      builder.Remove(builder.Length - 1, 1);
      builder.Append(Environment.NewLine);


      foreach (var entity in entities)
      {
        var row = this.PrepareEntityRow(sheet, entity);

        foreach (var rowCell in row.Cells)
        {
          string value = string.Empty;
          switch (rowCell.CellType)
          {
            case CellType.Unknown:
              break;
            case CellType.Numeric:
              value = rowCell.NumericCellValue.ToString(CultureInfo.CurrentCulture);
              break;
            case CellType.String:
              value = rowCell.StringCellValue;
              break;
            case CellType.Formula:
              break;
            case CellType.Blank:
              break;
            case CellType.Boolean:
              break;
            case CellType.Error:
              break;
            default:
              throw new ArgumentOutOfRangeException();
          }

          //value = value.Replace(@"""", @"");
          builder.Append($@"{value}; ");
        }

        builder.Remove(builder.Length - 1, 1);
        builder.Append(Environment.NewLine);
      }

      return builder.ToString();
    }

    protected virtual IRow PrepareEntityRow(ISheet sheet, TEntity entity)
    {
      var row = sheet.CreateRow(sheet.PhysicalNumberOfRows);
      var appendDef = PrepareEntityRow(entity);
      if (appendDef != null)
      {
        for (var index = 0; index < appendDef.Length; index++)
        {
          row.CreateCell(index).SetCellValue(appendDef[index]);
        }
      }
      else
      {
        row.CreateCell(0).SetCellValue(entity.Id);
      }

      return row;
    }

    protected virtual string[] PrepareEntityRow(TEntity entity)
    {
      return null;
    }

    public void ExportToExcel()
    {
      var workbook = new XSSFWorkbook();

      var preparedWorkBook = PrepareWorkBook(workbook);

      var dialog = new SaveFileDialog
      {
        InitialDirectory = @"~/Documents",
        Title = $"Путь к экспортируемой таблице {EntityName}",
        AddExtension = true,
        Filter = "Файлы Excel 2007 (*.xlsx)|*.xlsx|Все остальные файлы (*.*)|*.*"
      };
      if (dialog.ShowDialog() == DialogResult.OK)
      {
        if (!File.Exists(dialog.FileName))
        {
          File.Delete(dialog.FileName);
        }

        //запишем всё в файл
        using (var fs = new FileStream(dialog.FileName, FileMode.Create, FileAccess.Write))
        {
          preparedWorkBook.Write(fs);
        }
      }
    }

    public string EntityName { get; set; }

    public void RaiseStateChanged()
    {
      this.StateChanged?.Invoke();
    }
  }
}