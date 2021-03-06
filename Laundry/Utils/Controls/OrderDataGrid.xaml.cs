﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Controls;
using Caliburn.Micro;
using Model;
using Model.CollectionRepositories;
using Laundry.Utils.Controls.EntitySearchControls;
using Laundry.Views;
using LiveCharts;
using MaterialDesignThemes.Wpf;
using MongoDB.Driver;
using NPOI.XSSF.UserModel;
using System.Linq;
using Laundry.Utils.Converters;
using LiveCharts.Wpf;
using NPOI.SS.Util;
using PropertyChanged;
using NPOI.SS.UserModel;

namespace Laundry.Utils.Controls
{
  /// <summary>
  /// Interaction logic for OrderDataGrid.xaml
  /// </summary>
  public class OrderDataGridViewModel : EntityGrid<Order, OrderRepository, OrderCardViewModel>, IChartable<Order>
  {
    private readonly IEventAggregator _eventAggregator;
    private readonly IModel _model;
    private EmployeeProfession _profession;

    private readonly OrderStatusConverter _statusConverter = new OrderStatusConverter();
    private readonly MeasureKindConverter _measureKindConverter = new MeasureKindConverter();

    [AlsoNotifyFor(nameof(AggregatedInstancesCount), nameof(AggregatedPrice), nameof(AggregationResults))]
    public override IReadOnlyList<Order> Entities
    {
      get
      {
        return base.Entities;
      }

      set
      {
        base.Entities = value;
      }
    }

    public override FilterDefinition<Order> Filter
    {
      get
      {
        var filter = BaseFilter;

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

        if (this.IsByClient)
        {
          filter = Builders<Order>.Filter.And(
            filter,
            Builders<Order>.Filter.Eq(nameof(Order.ClientId), this.ClientCombo.SelectedEntity?.Id ?? -1));
        }

        if (this.IsByEmployee)
        {
          filter = Builders<Order>.Filter.And(
            filter,
            Builders<Order>.Filter.Or(
              Builders<Order>.Filter.Eq(nameof(Order.ObtainerId), this.EmployeeCombo.SelectedEntity?.Id ?? -1),
              Builders<Order>.Filter.Eq(nameof(Order.InCourierId), this.EmployeeCombo.SelectedEntity?.Id ?? -1),
              Builders<Order>.Filter.Eq(nameof(Order.WasherCourierId), this.EmployeeCombo.SelectedEntity?.Id ?? -1),
              Builders<Order>.Filter.Eq(nameof(Order.OutCourierId), this.EmployeeCombo.SelectedEntity?.Id ?? -1),
              Builders<Order>.Filter.Eq(nameof(Order.DistributerId), this.EmployeeCombo.SelectedEntity?.Id ?? -1)
            ));
        }

        if (IsBySubsidiary)
        {
          if (this.InSubsidiaryCombo.SelectedEntity != null)
          {
            filter = Builders<Order>.Filter.And(
              filter,
              Builders<Order>.Filter.Eq(nameof(Order.InSubsidiary), this.InSubsidiaryCombo.SelectedEntity.Id));
          }

          if (this.OutSubsidiaryCombo.SelectedEntity != null)
          {
            filter = Builders<Order>.Filter.And(
              filter,
              Builders<Order>.Filter.Eq(nameof(Order.OutSubsidiary), this.OutSubsidiaryCombo.SelectedEntity.Id));
          }
        }

        if (IsByPrice)
        {
          filter = Builders<Order>.Filter.And(
            filter,
            Builders<Order>.Filter.Gte(nameof(Order.Price), this.LowPriceBound ?? 0),
            Builders<Order>.Filter.Lte(nameof(Order.Price), this.TopPriceBound ?? double.MaxValue));
        }

        if (IsByStatus)
        {
          filter = Builders<Order>.Filter.And(
            filter,
            Builders<Order>.Filter.Eq(nameof(Order.Status), this.Status));
        }

        return filter;
      }

      set { base.Filter = value; }
    }

    #region Св-ва, описывающие поисковые параметры грида

    public bool IsByCreationDate { get; set; }
    public DateTime? LowCreationDateBound { get; set; }
    public DateTime? HighCreationDateBound { get; set; }

    public bool IsByExecutionDate { get; set; }
    public DateTime? LowExecutionDateBound { get; set; }
    public DateTime? HighExecutionDateBound { get; set; }

    public bool IsByClient { get; set; }
    public bool IsCorporative { get; set; }
    public ClientSearchViewModel ClientCombo { get; set; }

    public bool IsByEmployee { get; set; }

    public EmployeeProfession Profession
    {
      get { return _profession; }
      set
      {
        _profession = value;
        EmployeeCombo.Filter = Builders<Employee>.Filter.Eq(nameof(Employee.Profession), value);
      }
    }

    public EmployeeSearchViewModel EmployeeCombo { get; }

    public bool IsBySubsidiary { get; set; }
    public SubsidiarySearchViewModel InSubsidiaryCombo { get; set; }
    public SubsidiarySearchViewModel OutSubsidiaryCombo { get; set; }

    public bool IsByPrice { get; set; }
    public double? LowPriceBound { get; set; }
    public double? TopPriceBound { get; set; }

    public bool IsByStatus { get; set; }
    public OrderStatus Status { get; set; }

    #endregion


    public OrderDataGridViewModel(IEventAggregator eventAggregator, OrderCardViewModel card,
      DeleteDialogViewModel deleteDialog, IModel model, Visibilities visibilities
    ) : base(eventAggregator, card, model.Orders, deleteDialog, Screens.OrderEditor, visibilities)
    {
      _eventAggregator = eventAggregator;
      _model = model;
      eventAggregator.Subscribe(this);

      this.ClientCombo = new ClientSearchViewModel(model);

      this.InSubsidiaryCombo = new SubsidiarySearchViewModel(model, "Филиал принятия", null, false);
      this.OutSubsidiaryCombo = new SubsidiarySearchViewModel(model, "Филиал выдачи", null, false);

      this.EmployeeCombo = new EmployeeSearchViewModel(model) {Label = "Работник"};

      this.Profession = EmployeeProfession.Courier;
    }

    public override string TableSheetName => "Заказы";

    public override string[] TableSheetHeader =>
      new[]
      {
        "№", "Дата создания", "Дата исполнения", "Цена", "Статус", "Количество вешей", "Комментарий",
        "Клиент (№)", "Клиент (ФИО)",
        "Приёмщик (№)", "Приёмщик (ФИО)",
        "Корпоративный",
        "Забирающий курьер (№)", "Забирающий курьер (ФИО)",
        "Прачечник (№)", "Прачечник (ФИО)",
        "Доставляющий курьер (№)", "Доставляющий курьер (ФИО)",
        "Выдающий приёмщик (№)", "Количество вещей", "Список вещей"
      };

    protected override IRow PrepareEntityRow(ISheet sheet, Order entity)
    {
      #region Общая информация

      var row = sheet.CreateRow(sheet.PhysicalNumberOfRows);

      row.CreateCell(0).SetCellValue(entity.Id);
      row.CreateCell(1).SetCellValue(entity.CreationDate.ToString("d"));
      row.CreateCell(2).SetCellValue(entity.ExecutionDate.ToString("d"));

      row.CreateCell(3).SetCellValue(entity.Price);
      row.CreateCell(4).SetCellValue(_statusConverter
        .Convert(entity.Status, typeof(string), null, CultureInfo.CurrentCulture)?.ToString());
      row.CreateCell(5).SetCellValue(entity.InstancesCount);

      row.CreateCell(6).SetCellValue(entity.Comment);

      row.CreateCell(7).SetCellValue(entity.IsCorporative ? "Да" : "Нет");

      #endregion

      row.CreateCell(8).SetCellValue(entity.ClientId);
      row.CreateCell(9).SetCellValue(_model.Clients.GetById(entity.ClientId).ToString());

      #region Персонал

      row.CreateCell(10).SetCellValue(entity.ObtainerId);

      row.CreateCell(11).SetCellValue(entity.IsCorporative
        ? _model.Clients.GetById(entity.CorpObtainerId).ToString()
        : _model.Employees.GetById(entity.ObtainerId).ToString());

      row.CreateCell(12).SetCellValue(entity.InCourierId);
      row.CreateCell(13).SetCellValue(_model.Employees.GetById(entity.InCourierId).ToString());

      row.CreateCell(14).SetCellValue(entity.WasherCourierId);
      row.CreateCell(15).SetCellValue(_model.Employees.GetById(entity.WasherCourierId).ToString());

      row.CreateCell(16).SetCellValue(entity.OutCourierId);
      row.CreateCell(17).SetCellValue(_model.Employees.GetById(entity.OutCourierId).ToString());

      row.CreateCell(18).SetCellValue(entity.IsCorporative
        ? _model.Clients.GetById(entity.CorpDistributerId).ToString()
        : _model.Employees.GetById(entity.DistributerId).ToString());

      #endregion

      row.CreateCell(19).SetCellValue(entity.InstancesCount);


      var clothString = string.Empty;

      foreach (var instance in entity.Instances)
      {
        var measure = _measureKindConverter.Convert(instance.ClothKindObj.MeasureKind, typeof(string), null,
          CultureInfo.CurrentCulture);
        clothString += $"{instance.ClothKindObj.Name}: {instance.Amount} {measure}; ";
      }

      row.CreateCell(20).SetCellValue(clothString != String.Empty ? clothString : "Нет вещей");
      return row;
    }

    public string AggregatedInstancesCount => Repo.GetAggregatedInstacesCount(Filter);
    public double AggregatedPrice => Repo.GetAggregatedPrice(Filter);

    #region Св-ва, относящиеся к графикам
 

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
                Values = new ChartValues<long>(this.AggregationResults.Select(x => x.Count))
              },
              new ColumnSeries
              {
                Title = "кг",
                Values = new ChartValues<double>(this.AggregationResults.Select(x => x.UnCountableCount))
              }
            };
          case EntityInfoType.Cost:
            return new SeriesCollection
            {
              new ColumnSeries
              {
                Title = "₽",
                Values = new ChartValues<double>(this.AggregationResults.Select(x => x.Price))
              }
            };
          default:
            return null;
        }
      }
    }

    public IReadOnlyList<AggregationResult> AggregationResults => this.Repo.AggregateOrders(Time, Filter);

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

    #endregion
  }
}