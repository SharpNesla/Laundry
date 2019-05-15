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
using Laundry.Views.Cards;
using LiveCharts;
using LiveCharts.Wpf;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using PropertyChanged;

namespace Laundry.Utils.Controls
{
  /// <summary>
  /// Interaction logic for SubsidiaryGrid.xaml
  /// </summary>
  public class SubsidiaryGridViewModel : EntityGrid<Subsidiary, Repository<Subsidiary>, SubsidiaryCardViewModel>,
    IChartable<Subsidiary>
  {
    [AlsoNotifyFor(nameof(Labels), nameof(Values), nameof(Count))]
    public override IReadOnlyList<Subsidiary> Entities
    {
      get { return base.Entities; }

      set { base.Entities = value; }
    }

    private readonly IModel _model;

    public SubsidiaryGridViewModel(IEventAggregator eventAggregator, SubsidiaryCardViewModel card,
      DeleteDialogViewModel deleteDialog,
      IModel model) : base(eventAggregator, card, model.Subsidiaries, deleteDialog, Screens.SubsidiaryEditor)
    {
      _model = model;
    }


    public SeriesCollection Values => new SeriesCollection
    {
      new ColumnSeries
      {
        Title = "Сумма",
        Values = new ChartValues<double>(this.Repo.Get(0,int.MaxValue).Select(x => _model.Orders.GetAggregatedPriceForSubsidiary(x)))
      }
    };

    public override string[] TableSheetHeader => new[]
      {"№", "Торговое название", "Город", "Улица", "Дом", "Квартира (павильон)", "Почтовый индекс", "Номер телефона", "Главный приёмщик"};

    public override string TableSheetName => "Филиалы";

    protected override IRow PrepareEntityRow(ISheet sheet, Subsidiary entity)
    {
      var row = sheet.CreateRow(sheet.PhysicalNumberOfRows);
      
      row.CreateCell(0).SetCellValue(entity.Id);
      row.CreateCell(1).SetCellValue(entity.Name);
      row.CreateCell(2).SetCellValue(entity.City);
      row.CreateCell(3).SetCellValue(entity.Street);
      row.CreateCell(4).SetCellValue(entity.House);
      row.CreateCell(5).SetCellValue(entity.Flat ?? 0);
      row.CreateCell(6).SetCellValue(entity.ZipCode ?? 0);
      row.CreateCell(7).SetCellValue(entity.PhoneNumber);
      if (entity.MainAdvisor != null)
      {
        var mainAdvisor = this._model.Employees.GetById(entity.Id);
        row.CreateCell(8).SetCellValue(mainAdvisor.Signature);
      }
      

      return row;
    }

    public string[] Labels => this.Entities.Select(x => x.Signature).ToArray();
    public string LabelsTitle => "Филиалы";
    public string ValuesTitle => "Суммы";
    public ChartTime Time { get; set; }
    public EntityInfoType EntityInfoType { get; set; }
  }
}