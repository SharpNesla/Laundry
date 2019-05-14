using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Controls;
using Caliburn.Micro;
using Model;
using Model.CollectionRepositories;
using Model.DatabaseClients;
using Laundry.Utils.Converters;
using Laundry.Views;
using LiveCharts;
using MaterialDesignThemes.Wpf;
using MongoDB.Driver;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace Laundry.Utils.Controls
{
  /// <summary>
  /// Interaction logic for OrderDataGrid.xaml
  /// </summary>
  public class EmployeeDataGridViewModel : EntityGrid<Employee, EmployeeRepository, EmployeeCardViewModel>
  {
    private readonly ProfessionConverter _employeeProfessionConverter = new ProfessionConverter();

    public bool IsByDateBirth { get; set; }
    public DateTime? LowDateBirthBound { get; set; }
    public DateTime? HighDateBirthBound { get; set; }

    public bool IsByOrdersCount { get; set; }
    public int? LowOrdersCountBound { get; set; }
    public int? TopOrdersCountBound { get; set; }

    public bool IsByOrdersPrice { get; set; }
    public int? LowOrdersPriceBound { get; set; }
    public int? TopOrdersPriceBound { get; set; }

    public bool IsByGender { get; set; }
    public Gender Gender { get; set; }

    public override FilterDefinition<Employee> Filter
    {
      get
      {
        var filter = BaseFilter;

        if (this.IsByDateBirth)
        {
          filter = Builders<Employee>.Filter.And(
            filter,
            Builders<Employee>.Filter.Gte(nameof(Employee.DateBirth), this.LowDateBirthBound ?? DateTime.MinValue),
            Builders<Employee>.Filter.Lte(nameof(Employee.DateBirth), this.HighDateBirthBound ?? DateTime.MaxValue));
        }

        if (this.IsByOrdersCount)
        {
          filter = Builders<Employee>.Filter.And(
            filter,
            Builders<Employee>.Filter.Gte(nameof(Client.OrdersCount), this.LowOrdersCountBound ?? int.MinValue),
            Builders<Employee>.Filter.Lte(nameof(Client.OrdersCount), this.TopOrdersCountBound ?? int.MaxValue));
        }

        if (this.IsByOrdersPrice)
        {
          filter = Builders<Employee>.Filter.And(
            filter,
            Builders<Employee>.Filter.Gte(nameof(Client.OrdersPrice), this.LowOrdersPriceBound ?? int.MinValue),
            Builders<Employee>.Filter.Lte(nameof(Client.OrdersPrice), this.TopOrdersPriceBound ?? int.MaxValue));
        }

        if (this.IsByGender)
        {
          filter = Builders<Employee>.Filter.And(
            filter,
            Builders<Employee>.Filter.Eq(nameof(Employee.Gender), Gender));
        }

        return filter;
      }

      set { base.Filter = value; }
    }

    public EmployeeDataGridViewModel(IEventAggregator eventAggregator, EmployeeCardViewModel card,
      DeleteDialogViewModel deleteDialog, IModel model, Visibilities visibilities)
      : base(eventAggregator, card, model.Employees, deleteDialog, Screens.EmployeeEditor, visibilities)
    {
    }
    public override string TableSheetName => "Работники";
    public override string[] TableSheetHeader => new[]
      {"№", "Имя", "Фамилия", "Отчество", "Номер телефона","Должность" ,"Дата рождения", "Количество заказов", /*"Общая сумма всех заказов"*/};

    protected override IRow PrepareEntityRow(ISheet sheet, Employee entity)
    {
      var row = sheet.CreateRow(sheet.PhysicalNumberOfRows);

      row.CreateCell(0).SetCellValue(entity.Id);
      row.CreateCell(1).SetCellValue(entity.Surname);
      row.CreateCell(2).SetCellValue(entity.Name);
      row.CreateCell(3).SetCellValue(entity.Patronymic);
      row.CreateCell(4).SetCellValue(entity.PhoneNumber);
      row.CreateCell(5).SetCellValue(this._employeeProfessionConverter.Convert(entity.Profession,
        typeof(string), null, CultureInfo.CurrentCulture) as string);
      row.CreateCell(6).SetCellValue(entity.DateBirth.ToString("dd.MM.yyyy"));
      row.CreateCell(7).SetCellValue(entity.OrdersCount);
      //row.CreateCell(8).SetCellValue(entity.OrdersPrice);
      return row;
    }
  }
}