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
            Builders<Client>.Filter.Lte(nameof(Client.DateBirth), this.HighDateBirthBound?? DateTime.MaxValue));
        }

        return filter;
      }

      set { base.Filter = value; }
    }

    public ClientDataGridViewModel(IEventAggregator eventAggregator, ClientCardViewModel card,
      DeleteDialogViewModel deleteDialog, IModel model) :
      base(eventAggregator, card, model.Clients, deleteDialog, Screens.ClientEditor)
    {
    }

    public override void ExportToExcel()
    {
      var workbook = new XSSFWorkbook();

      var sheet = workbook.CreateSheet();

      var clients = this.Repo.Get(0, int.MaxValue);

      foreach (var client in clients)
      {
        sheet.AppendClient(client);
      }

      var dialog = new SaveFileDialog
      {
        InitialDirectory = @"~/Documents",
        Title = "Путь к экспортируемой таблице клиентов",
        AddExtension = true,
        Filter = "Файлы Excel 2007 (*.xlsx)|*.xlsx|Все остальные файлы (*.*)|*.*"
      };
      if (dialog.ShowDialog() == DialogResult.OK)
      {
        if (!File.Exists(dialog.FileName))
        {
          File.Delete(dialog.FileName);
        }

        //запишем всё в файл
        using (var fs = new FileStream(dialog.FileName, FileMode.Create, FileAccess.Write))
        {
          workbook.Write(fs);
        }
      }
    }
  }
}