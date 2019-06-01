using System;
using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using Model;
using Model.CollectionRepositories;
using Laundry.Views;
using Laundry.Views.Cards;
using LiveCharts;
using LiveCharts.Wpf;
using MongoDB.Driver;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using PropertyChanged;

namespace Laundry.Utils.Controls
{
  /// <summary>
  /// Таблица филиалов
  /// </summary>
  public class SubsidiaryGridViewModel : EntityGrid<Subsidiary, Repository<Subsidiary>, SubsidiaryCardViewModel>,
    IChartable<Subsidiary>
  {
    #region Специальные фильтры дат для аналитики

    public bool IsByCreationDate { get; set; }
    public DateTime? LowCreationDateBound { get; set; }
    public DateTime? HighCreationDateBound { get; set; }

    public bool IsByExecutionDate { get; set; }
    public DateTime? LowExecutionDateBound { get; set; }
    public DateTime? HighExecutionDateBound { get; set; }
    
    public FilterDefinition<Order> GetDateFilters()
    {
      var filter = Builders<Order>.Filter.Empty;
      if (this.IsByCreationDate)
      {
        filter = Builders<Order>.Filter.And(
          filter,
          Builders<Order>.Filter.Gte(nameof(Order.CreationDate), this.LowCreationDateBound ?? DateTime.MinValue),
          Builders<Order>.Filter.Lte(nameof(Order.CreationDate), this.HighCreationDateBound ?? DateTime.MaxValue));
      }

      if (this.IsByExecutionDate)
      {
        filter = Builders<Order>.Filter.And(
          filter,
          Builders<Order>.Filter.Gte(nameof(Order.ExecutionDate), this.LowExecutionDateBound ?? DateTime.MinValue),
          Builders<Order>.Filter.Lte(nameof(Order.ExecutionDate), this.HighExecutionDateBound ?? DateTime.MaxValue));
      }

      return filter;
    }

      #endregion

    [AlsoNotifyFor(nameof(Labels), nameof(Values), nameof(Count))]
    public override IReadOnlyList<Subsidiary> Entities
    {
      get { return base.Entities; }

      set { base.Entities = value; }
    }

    private readonly IModel _model;


    public SubsidiaryGridViewModel(IEventAggregator eventAggregator, SubsidiaryCardViewModel card,
      DeleteDialogViewModel deleteDialog, IModel model, Visibilities visibilities) :
      base(eventAggregator, card, model.Subsidiaries, deleteDialog, Screens.SubsidiaryEditor, visibilities)
    {
      _model = model;
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
                Values = new ChartValues<long>(this.Type.Select(x => x.Count))
              },
              new ColumnSeries
              {
                Title = "кг",
                Values = new ChartValues<double>(this.Type.Select(x => x.UnCountableCount))
              }
            };
          case EntityInfoType.Cost:
            return new SeriesCollection
            {
              new ColumnSeries
              {
                Title = "₽",
                Values = new ChartValues<double>(this.Type.Select(x => x.Price))
              }
            };
          default:
            return null;
        }
      }
    }

    public override string[] TableSheetHeader => new[]
    {
      "№", "Торговое название", "Город", "Улица", "Дом", "Квартира (павильон)", "Почтовый индекс", "Номер телефона",
      "Главный приёмщик"
    };

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

    public IReadOnlyList<SubsidiaryAggregationResult> Type =>
      this._model.Subsidiaries.AggregateSubsidiaries(GetDateFilters());
    
    public string AggregatedInstancesCount => _model.Orders.GetAggregatedInstacesCount(GetDateFilters());
    public double AggregatedPrice => _model.Orders.GetAggregatedPrice(GetDateFilters());

    public string[] Labels => Type.Select(x => x.Signature).ToArray();
    public string LabelsTitle => "Филиалы";
    public string ValuesTitle => "Суммы";
    public ChartTime Time { get; set; }
    public EntityInfoType EntityInfoType { get; set; }
  }
}