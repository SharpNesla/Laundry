using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
using Laundry.Utils.Converters;
using Laundry.Views;
using Laundry.Views.Cards;
using LiveCharts;
using LiveCharts.Wpf;
using MongoDB.Driver;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using PropertyChanged;

namespace Laundry.Utils.Controls
{
  /// <summary>
  /// Interaction logic for ClothKindGrid.xaml
  /// </summary>
  public class ClothKindTreeViewModel : EntityGrid<ClothKind, ClothKindRepository, ClothKindCardViewModel>,
    IChartable<ClothKind>
  {
    #region Фильтры

    public bool IsByCount { get; set; }
    public double? LowCountBound { get; set; }
    public double? TopCountBound { get; set; }

    public bool IsBySumPrice { get; set; }
    public int? LowSumPriceBound { get; set; }
    public int? TopSumPriceBound { get; set; }

    public override FilterDefinition<ClothKind> Filter
    {
      get
      {
        var filter = BaseFilter;

        if (this.IsBySumPrice)
        {
          filter = Builders<ClothKind>.Filter.And(
            filter,
            Builders<ClothKind>.Filter.Gte(nameof(ClothKind.SumPrice), this.LowSumPriceBound ?? 0),
            Builders<ClothKind>.Filter.Lte(nameof(ClothKind.SumPrice), this.TopSumPriceBound ?? double.MaxValue));
        }

        if (this.IsByCount)
        {
          filter = Builders<ClothKind>.Filter.And(
            filter,
            Builders<ClothKind>.Filter.Gte(nameof(ClothKind.Count), this.LowCountBound ?? 0),
            Builders<ClothKind>.Filter.Lte(nameof(ClothKind.Count), this.TopCountBound?? int.MaxValue));
        }

        return filter;
      }

      set { base.Filter = value; }
    }

    private readonly MeasureKindConverter _measureKindConverter = new MeasureKindConverter();
    private readonly IModel _model;

    #endregion

    public float NameWidth { get; set; }
    public ObservableCollection<ClothKind> EditableEntities { get; private set; }
    public ClothKind SelectedTreeEntity { get; set; }
    public bool IsTreeMode { get; set; }

    public ClothKindTreeViewModel(IEventAggregator eventAggregator, ClothKindCardViewModel card,
      IModel model, DeleteDialogViewModel removeDialog, Visibilities visibilities)
      : base(eventAggregator, card, model.ClothKinds, removeDialog, Screens.ClothKindEditor, visibilities)
    {
      _model = model;
      this.EditableEntities = new ObservableCollection<ClothKind> {Repo.GetById(0)};
    }

    public override async void Add()
    {
      var editor = new ClothKindEditorViewModel(this.EventAggregator, _model);
      editor.ClothKindParent = this.SelectedEntity;
      await DialogHostExtensions.ShowCaliburnVM(editor);
      this.Refresh(0, 0);
    }

    public override async void Edit()
    {
      var editor = new ClothKindEditorViewModel(this.EventAggregator, _model);
      EventAggregator.PublishOnUIThread(this.SelectedEntity);
      await DialogHostExtensions.ShowCaliburnVM(editor);
      RaiseStateChanged();
    }
    public override string TableSheetName => "Виды одежды";
    public override string[] TableSheetHeader => new[] {"№", "Название", "Цена", "Единица измерения", "Единиц одежды", "Общая стоимость"};

    protected override IRow PrepareEntityRow(ISheet sheet, ClothKind entity)
    {
      var row = sheet.CreateRow(sheet.PhysicalNumberOfRows);
      row.CreateCell(0).SetCellValue(entity.Id);
      row.CreateCell(1).SetCellValue(entity.Name);
      row.CreateCell(2).SetCellValue(entity.Price);
      row.CreateCell(3).SetCellValue(_measureKindConverter.Convert(entity.MeasureKind, typeof(string), null, CultureInfo.CurrentCulture)?.ToString());
      row.CreateCell(5).SetCellValue(entity.Count);
      row.CreateCell(4).SetCellValue(entity.SumPrice);
      
      return row;
    }

    public void ShowHideDetails(ToggleButton button, ClothKind clothKind, ClothKindTreeView view)
    {
      if (button.IsChecked.Value)
      {
        this.Repo.FetchChildren(clothKind);

        foreach (var kind in clothKind.Children)
        {
          kind.Level = clothKind.Level + 1;
          this.EditableEntities.Insert(EditableEntities.IndexOf(clothKind) + 1, kind);
        }

        //view.MainGrid.Columns[0].Width = new DataGridLength((clothKind.Level + 1) * 64, DataGridLengthUnitType.Pixel);
      }

      else
      {
        RemoveChildren(clothKind);

        //view.MainGrid.Columns[0].Width = new DataGridLength((clothKind.Level) * 64 + 64, DataGridLengthUnitType.Pixel);
      }
    }

    public void RemoveChildren(ClothKind clothKind)
    {
      if (clothKind.HasChildren && clothKind.Children != null)
      {
        foreach (var kind in clothKind.Children)
        {
          this.EditableEntities.Remove(kind);
          RemoveChildren(kind);
        }
      }
    }

    public string AggregatedInstancesCount => Repo.GetAggregatedInstacesCount(Filter);
    public double AggregatedPrice => Repo.GetAggregatedPrice(Filter);

    public override void Refresh(int page, int elements)
    {
      base.Refresh(page, elements);
      this.EditableEntities = new ObservableCollection<ClothKind> {Repo.GetById(0)};
    }


    public SeriesCollection Values
    {

      get
      {
        switch (this.EntityInfoType)
        {
          case EntityInfoType.Amount:
            return new SeriesCollection
            {
              new ColumnSeries
              {
                Title = "шт",
                Values = new ChartValues<long>(this.AggregationResults.Select(x=> x.Count))
              },
              new ColumnSeries
              {
                Title = "кг",
                Values = new ChartValues<double>(this.AggregationResults.Select(x=> x.UnCountableCount))
              }
            };
          case EntityInfoType.Cost:
            return new SeriesCollection
            {
              new ColumnSeries
              {
                Title = "₽",
                Values = new ChartValues<double>(this.AggregationResults.Select(x=> x.Price))
              }
            };
          default:
            return null;
        }
      }
    }

    public IReadOnlyList<AggregationResult> AggregationResults => 
      this.Repo.AggregateInstances(Time, Filter);

    public string[] Labels
    {
      get
      {
        switch (Time)
        {
          case ChartTime.Day:
            return this.AggregationResults.Select(x => x.DateTime.ToString("d")).ToArray();
          case ChartTime.Mounth:
            return this.AggregationResults.Select(x => x.DateTime.ToString("y")).ToArray();
          case ChartTime.Year:
            return this.AggregationResults.Select(x => x.DateTime.ToString("yyyy")).ToArray();
          default:
            return null;
        }
      }
    }
    [AlsoNotifyFor(nameof(Labels))]
    public ChartTime Time { get; set; }
    public EntityInfoType EntityInfoType { get; set; }
  }
}