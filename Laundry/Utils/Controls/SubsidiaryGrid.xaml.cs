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
using NPOI.XSSF.UserModel;

namespace Laundry.Utils.Controls
{
  /// <summary>
  /// Interaction logic for SubsidiaryGrid.xaml
  /// </summary>
  public class SubsidiaryGridViewModel : EntityGrid<Subsidiary, Repository<Subsidiary>, SubsidiaryCardViewModel>, IChartable<Subsidiary>
  {
    public SubsidiaryGridViewModel(IEventAggregator eventAggregator, SubsidiaryCardViewModel card,
      DeleteDialogViewModel deleteDialog,
      IModel model) : base(eventAggregator, card, model.Subsidiaries, deleteDialog, Screens.SubsidiaryEditor)
    {
    }

    protected override XSSFWorkbook PrepareWorkBook(XSSFWorkbook workbook)
    {
      throw new NotImplementedException();
    }

    public SeriesCollection Values { get; }
    public string[] Labels { get; }
    public string LabelsTitle { get; }
    public string ValuesTitle { get; }
  }
}