using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Caliburn.Micro;
using Model;
using Model.DatabaseClients;
using Laundry.Utils.Controls.EntitySearchControls;
using Laundry.Views;
using MongoDB.Driver;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;

namespace Laundry.Utils.Controls
{
  /// <summary>
  /// Interaction logic for ClientDataGrid.xaml
  /// </summary>
  public class ClientDataGridViewModel : EntityGrid<Client, ClientRepository, ClientCardViewModel>
  {
    public Client Client { get; set; }

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

    public override FilterDefinition<Client> Filter
    {
      get
      {
        var filter = BaseFilter;

        if (this.IsByDateBirth)
        {
          filter = Builders<Client>.Filter.And(
            filter,
            Builders<Client>.Filter.Gte(nameof(Client.DateBirth), this.LowDateBirthBound ?? DateTime.MinValue),
            Builders<Client>.Filter.Lte(nameof(Client.DateBirth), this.HighDateBirthBound ?? DateTime.MaxValue));
        }

        if (this.IsByOrdersCount)
        {
          filter = Builders<Client>.Filter.And(
            filter,
            Builders<Client>.Filter.Gte(nameof(Client.OrdersCount), this.LowOrdersCountBound ?? int.MinValue),
            Builders<Client>.Filter.Lte(nameof(Client.OrdersCount), this.TopOrdersCountBound ?? int.MaxValue));
        }

        if (this.IsByOrdersPrice)
        {
          filter = Builders<Client>.Filter.And(
            filter,
            Builders<Client>.Filter.Gte(nameof(Client.OrdersPrice), this.LowOrdersPriceBound ?? int.MinValue),
            Builders<Client>.Filter.Lte(nameof(Client.OrdersPrice), this.TopOrdersPriceBound ?? int.MaxValue));
        }

        if (this.IsByGender)
        {
          filter = Builders<Client>.Filter.And(
            filter,
            Builders<Client>.Filter.Eq(nameof(Client.Gender), Gender));
        }

        return filter;
      }

      set { base.Filter = value; }
    }

    public override string[] TableSheetHeader => new[]
      {"№", "Имя", "Фамилия", "Отчество", "Номер телефона", "Дата рождения", "Количество заказов", "Общая сумма всех заказов"};

    public override string TableSheetName => "Клиенты";

    protected override IRow PrepareEntityRow(ISheet sheet, Client entity)
    {
      var row = sheet.CreateRow(sheet.PhysicalNumberOfRows);

      row.CreateCell(0).SetCellValue(entity.Id);
      row.CreateCell(1).SetCellValue(entity.Surname);
      row.CreateCell(2).SetCellValue(entity.Name);
      row.CreateCell(3).SetCellValue(entity.Patronymic);
      row.CreateCell(4).SetCellValue(entity.PhoneNumber);
      row.CreateCell(5).SetCellValue(entity.DateBirth.ToString("dd.MM.yyyy"));
      row.CreateCell(6).SetCellValue(entity.OrdersCount);
      row.CreateCell(7).SetCellValue(entity.OrdersPrice);
      return row;
    }

    public ClientDataGridViewModel(IEventAggregator eventAggregator, ClientCardViewModel card,
      DeleteDialogViewModel deleteDialog, IModel model, Visibilities visibilities) :
      base(eventAggregator, card, model.Clients, deleteDialog, Screens.ClientEditor, visibilities)
    {
    }
  }
}