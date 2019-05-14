using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Caliburn.Micro;
using Model;
using Model.CollectionRepositories;
using Laundry.Views;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace Laundry.Utils.Controls
{
  public class DiscountGridViewModel : EntityGrid<DiscountEdge, DiscountSystemRepository, Card<DiscountEdge>>
  {
    private readonly IModel _model;
    private readonly IEventAggregator _eventAggregator;

    public DiscountGridViewModel(IModel model, IEventAggregator eventAggregator, DeleteDialogViewModel deleteDialog) :
      base(eventAggregator, null, model.DiscountEdges, deleteDialog, Screens.About)
    {
      _model = model;
      _eventAggregator = eventAggregator;
    }

    public override async void Add()
    {
      var editor = new DiscountEdgeEditorViewModel(_eventAggregator, _model);
      await DialogHostExtensions.ShowCaliburnVM(editor);
      RaiseStateChanged();
    }

    public override async void Edit()
    {
      var editor = new DiscountEdgeEditorViewModel(_eventAggregator, _model);
      EventAggregator.PublishOnUIThread(this.SelectedEntity);
      await DialogHostExtensions.ShowCaliburnVM(editor);
      RaiseStateChanged();
    }

    public async void Edit(DiscountEdge edge)
    {
      this.SelectEdge(edge);
      this.Edit();
    }

    public void SelectEdge(DiscountEdge edge)
    {
      this.SelectedEntity = edge;
    }

    protected override IRow PrepareEntityRow(ISheet sheet, DiscountEdge entity)
    {
      var row = sheet.CreateRow(sheet.PhysicalNumberOfRows);

      row.CreateCell(0).SetCellValue(entity.Id);
      row.CreateCell(1).SetCellValue(entity.Edge);
      row.CreateCell(2).SetCellValue($"{entity.Discount}%");

      return row;
    }

    public override string[] TableSheetHeader => new[] {"№", "Граница", "Величина скидки"};
    public override string TableSheetName => "Границы дисконт-системы";
  }
}