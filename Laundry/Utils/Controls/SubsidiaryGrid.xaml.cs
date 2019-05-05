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
using Laundry.Model;
using Laundry.Model.CollectionRepositories;
using Laundry.Views;
using Laundry.Views.Cards;
using LiveCharts;
using LiveCharts.Wpf;
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

    protected override XSSFWorkbook PrepareWorkBook(XSSFWorkbook workbook)
    {
      throw new NotImplementedException();
    }

    public SeriesCollection Values => new SeriesCollection
    {
      new ColumnSeries
      {
        Title = "Сумма",
        Values = new ChartValues<double>(this.Entities.Select(x => _model.Orders.GetAggregatedPriceForSubsidiary(x)))
      }
    };

    public string[] Labels => this.Entities.Select(x => x.Signature).ToArray();
    public string LabelsTitle => "Филиалы";
    public string ValuesTitle => "Суммы";
    public ChartTime Time { get; set; }
    public EntityInfoType EntityInfoType { get; set; }
  }
}