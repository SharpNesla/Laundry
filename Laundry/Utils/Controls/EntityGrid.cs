using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Caliburn.Micro;
using Laundry.Model;
using Laundry.Model.CollectionRepositories;
using Laundry.Model.DatabaseClients;
using Laundry.Views;
using MaterialDesignThemes.Wpf;
using MongoDB.Driver;
using NPOI.SS.UserModel;
using Action = System.Action;
using NPOI.XSSF.UserModel;

namespace Laundry.Utils.Controls
{
  public interface IEntityGrid<out TEntity> : IPaginable where TEntity : IRepositoryElement
  {
    IReadOnlyList<TEntity> Entities { get; }
    TEntity SelectedEntity { get; }

    void ExportToExcel();
    void Add();
    void Edit();
    void Remove();
    void RemoveSelectedGroup();
    void Refresh();
  }

  public abstract class EntityGrid<TEntity, TRepository, TCard> : PropertyChangedBase, IEntityGrid<TEntity>
    where TEntity : IRepositoryElement
    where TRepository : Repository<TEntity>
    where TCard : Card<TEntity>
  {
    private Card<TEntity> _card;
    protected readonly IEventAggregator EventAggregator;
    private Screens _editScreen;
    public IReadOnlyList<TEntity> Entities { get; set; }
    public TEntity SelectedEntity { get; set; }

    public IReadOnlyList<TEntity> SelectedEntities
    {
      get { return this.Entities.Where(x => x.IsSelected).ToList(); }
    }

    public virtual FilterDefinition<TEntity> Filter
    {
      get { return BaseFilter; }
      set { BaseFilter = value; }
    }

    private readonly DeleteDialogViewModel _shure;

    protected FilterDefinition<TEntity> BaseFilter { get; private set; }
    public TRepository Repo { get; }

    public Visibilities Visibilities { get; }

    public bool IsSearchDrawerOpened { get; set; }

    public bool DisplaySelectionColumn { get; set; }

    public event Action<TEntity> RemoveButtonClick;

    public EntityGrid(IEventAggregator eventAggregator, TCard card, TRepository repo,
      DeleteDialogViewModel shure, Screens editScreen, Visibilities visibilities = null, string entityName = "объекта",
      bool displaySelectColumn = true)
    {
      this._card = card;
      this._editScreen = editScreen;
      this.EventAggregator = eventAggregator;
      this.Repo = repo;
      this.EntityName = entityName;
      this._shure = shure;
      this.DisplaySelectionColumn = displaySelectColumn;
      this.BaseFilter = Builders<TEntity>.Filter.Empty;
      this.Visibilities = visibilities;
    }

    public virtual long Count => this.Repo.GetCount(Filter);

    public virtual void Refresh(int page, int elements)
    {
      this.Entities = Repo.Get(page * elements, elements, Filter);
    }

    public event Action StateChanged;

    public void ShowInfoCard()
    {
      if (SelectedEntity != null)
      {
        _card.Entity = SelectedEntity;

        DialogHostExtensions.ShowCaliburnVM(_card);
      }
    }

    public async void ShowInfoCard(TEntity context)
    {
      if (context != null)
      {
        _card.Entity = context;

        await DialogHostExtensions.ShowCaliburnVM(_card);
      }
    }

    public virtual void Add()
    {
      EventAggregator.PublishOnUIThread(_editScreen);
      StateChanged?.Invoke();
    }

    public virtual void Edit()
    {
      EventAggregator.PublishOnUIThread(_editScreen);
      EventAggregator.PublishOnUIThread(SelectedEntity);
    }

    public async void Remove()
    {
      var isDelete = await _shure.AskQuestion();
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

    protected abstract XSSFWorkbook PrepareWorkBook(XSSFWorkbook workbook);

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

    public object EntityName { get; set; }

    public void RaiseStateChanged()
    {
      this.StateChanged?.Invoke();
    }
  }

  public static class ExcelExtensions
  {
    public static void AppendClient(this ISheet sheet, Client client)
    {
      var row = sheet.CreateRow(sheet.PhysicalNumberOfRows);
      var indexCell = row.CreateCell(0);
      var nameCell = row.CreateCell(2);
      var surnameCell = row.CreateCell(1);

      row.CreateCell(3).SetCellValue(client.Patronymic);

      var datebirth = row.CreateCell(4);

      row.CreateCell(5).SetCellValue(client.OrdersCount);

      indexCell.SetCellValue(client.Id);
      nameCell.SetCellValue(client.Name);
      surnameCell.SetCellValue(client.Surname);
      datebirth.SetCellValue(client.DateBirth.ToString("dd.MM.yyyy"));
    }
  }
}