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
    void Refresh();
  }

  public abstract class EntityGrid<TEntity, TRepository, TCard> : PropertyChangedBase, IEntityGrid<TEntity>
    where TEntity : IRepositoryElement
    where TRepository : Repository<TEntity>
    where TCard : Card<TEntity>
  {
    private Card<TEntity> _card;
    private IEventAggregator _eventAggregator;
    private Screens _editScreen;
    public IReadOnlyList<TEntity> Entities { get; set; }
    public TEntity SelectedEntity { get; set; }

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
      DeleteDialogViewModel shure, Screens editScreen,Visibilities visibilities = null, bool displaySelectColumn = true)
    {
      this._card = card;
      this._editScreen = editScreen;
      this._eventAggregator = eventAggregator;
      this.Repo = repo;
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

    public void ShowInfoCard(TEntity context)
    {
      if (context != null)
      {
        _card.Entity = context;

        DialogHostExtensions.ShowCaliburnVM(_card);
      }
    }

    public virtual void Add()
    {
      _eventAggregator.PublishOnUIThread(_editScreen);
      StateChanged?.Invoke();
    }

    public virtual void Edit()
    {
      _eventAggregator.PublishOnUIThread(_editScreen);
      _eventAggregator.PublishOnUIThread(SelectedEntity);
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

    public abstract void ExportToExcel();

    public void RaiseStateChanged()
    {
      this.StateChanged?.Invoke();
    }

    private void chkItems_CheckedChanged(object sender, EventArgs e)
    {
      //foreach (DataGridViewRow row in GridView1.Rows)
      //{
      //  DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells[1];
      //  if (chk.Selected == false)
      //  {
      //    chk.Selected = true;
      //  }
      //}
    }
  }

  public static class ExcelExtensions
  {
    public static void AppendClient(this ISheet sheet, Client client)
    {
      var row = sheet.CreateRow(sheet.PhysicalNumberOfRows + 1);
      var indexCell = row.CreateCell(0);
      var nameCell = row.CreateCell(1);
      var surnameCell = row.CreateCell(2);

      indexCell.SetCellValue(client.Id);
      nameCell.SetCellValue(client.Name);
      surnameCell.SetCellValue(client.Surname);

      
    }
  }
}