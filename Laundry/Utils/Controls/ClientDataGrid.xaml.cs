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
using Laundry.Model;
using Laundry.Model.DatabaseClients;
using Laundry.Utils.Controls.EntitySearchControls;
using Laundry.Views;
using MongoDB.Driver;
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

    public override FilterDefinition<Client> Filter
    {
      get
      {
        var filter = BaseFilter;

        if (this.IsByDateBirth)
        {
          filter = Builders<Client>.Filter.And(
            this.BaseFilter,
            Builders<Client>.Filter.Gte(nameof(Client.DateBirth), this.LowDateBirthBound ?? DateTime.MinValue),
            Builders<Client>.Filter.Lte(nameof(Client.DateBirth), this.HighDateBirthBound ?? DateTime.MaxValue));
        }

        return filter;
      }

      set { base.Filter = value; }
    }

    protected override XSSFWorkbook PrepareWorkBook(XSSFWorkbook workbook)
    {
      var sheet = workbook.CreateSheet();

      var clients = this.Repo.Get(0, int.MaxValue);

      workbook.SetSheetName(0, "Клиенты");

      var header = sheet.CreateRow(0);
      header.CreateCell(0).SetCellValue("№");
      header.CreateCell(1).SetCellValue("Имя");
      header.CreateCell(2).SetCellValue("Фамилия");
      header.CreateCell(3).SetCellValue("Отчество");
      header.CreateCell(4).SetCellValue("Дата рождения");
      header.CreateCell(5).SetCellValue("Количество заказов");

      foreach (var client in clients)
      {
        sheet.AppendClient(client);
      }

      

      for (int i = 0; i < 6; i++)
      {
        sheet.AutoSizeColumn(i);
        
        sheet.SetAutoFilter(new CellRangeAddress(0, sheet.PhysicalNumberOfRows, i, i));
      }

      return workbook;
    }

    public ClientDataGridViewModel(IEventAggregator eventAggregator, ClientCardViewModel card,
      DeleteDialogViewModel deleteDialog, IModel model) :
      base(eventAggregator, card, model.Clients, deleteDialog, Screens.ClientEditor)
    {
    }
  }
}