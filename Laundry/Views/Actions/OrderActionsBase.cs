using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Laundry.Model;
using Laundry.Model.CollectionRepositories;
using Laundry.Utils;
using Laundry.Utils.Controls;
using MongoDB.Driver;
using NPOI.XWPF.UserModel;
using Screen = Caliburn.Micro.Screen;

namespace Laundry.Views.Actions
{
  public abstract class OrderActionsBase : Screen
  {
    protected readonly OrderRepository Repository;
    private readonly OrderStatus _changingStatus;
    public OrderDataGridViewModel OrderGrid { get; set; }

    public OrderActionsBase(OrderRepository orderRepo, Employee currentUser, string orderEmployeeInvolvement, OrderDataGridViewModel orderGrid,
      OrderStatus startStatus, OrderStatus changingStatus)
    {
      Repository = orderRepo;
      _changingStatus = changingStatus;
      OrderGrid = orderGrid;
      OrderGrid.Filter =
        Builders<Order>.Filter.And(
          Builders<Order>.Filter.Eq(nameof(Order.Status), startStatus),
          Builders<Order>.Filter.Eq(orderEmployeeInvolvement, currentUser.Id));
      this.OrderGrid.Refresh(0, int.MaxValue);
    }

    public virtual async void Apply()
    {
      this.Repository.SetOrdersStatus(this.OrderGrid.SelectedEntities, _changingStatus);

      foreach (var order in this.OrderGrid.SelectedEntities)
      {
        this.WriteDocumentation(order);
      }
    }

    public abstract Document PrepareDocument(Document document, Order order);

    private void WriteDocumentation(Order order)
    {
      XWPFDocument document = null;
      try
      {
        using (FileStream file = new FileStream("Resources/Bill.docx", FileMode.Open, FileAccess.Read))
        {
          document = new XWPFDocument(file);
        }
      }
      catch (Exception e)
      {
        throw e;
      }

      if (document != null)
      {
        PrepareDocument(document, order);

        var dialog = new SaveFileDialog
        {
          InitialDirectory = @"~/Documents",
          Title = $"Путь к экспортируемому документу",
          AddExtension = true,
          Filter = "Файлы Word 2007 (*.docx)|*.docx|Все остальные файлы (*.*)|*.*"
        };
        if (dialog.ShowDialog() == DialogResult.OK)
        {
          if (!File.Exists(dialog.FileName))
          {
            File.Delete(dialog.FileName);
          }

          using (var fs = new FileStream(dialog.FileName, FileMode.Create, FileAccess.Write))
          {
            document.Write(fs);
          }
        }
      }

      
    }
  }
}