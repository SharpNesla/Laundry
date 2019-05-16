using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Model;
using Model.CollectionRepositories;
using Laundry.Utils;
using Laundry.Utils.Controls;
using Laundry.Utils.Converters;
using MongoDB.Driver;
using NPOI.OpenXmlFormats.Wordprocessing;
using NPOI.XWPF.UserModel;
using Screen = Caliburn.Micro.Screen;

namespace Laundry.Views.Actions
{
  public abstract class OrderActionsBase : Screen
  {
    protected readonly OrderRepository Repository;
    private readonly OrderStatus _changingStatus;
    private readonly string _documentName;
    public OrderDataGridViewModel OrderGrid { get; set; }
    private readonly OrderStatusConverter _converter = new OrderStatusConverter();
    private readonly MeasureKindConverter _measureKindConverter = new MeasureKindConverter();

    public OrderActionsBase(OrderRepository orderRepo, Employee currentUser, string orderEmployeeInvolvement,
      OrderDataGridViewModel orderGrid,
      OrderStatus startStatus, OrderStatus changingStatus, string documentName = "Bill.docx")
    {
      Repository = orderRepo;
      _changingStatus = changingStatus;
      _documentName = documentName;
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
    /// <summary>
    /// Подготовка документа для дальнейшего экспорта (накладная, договоры и.т.п.)
    /// </summary>
    /// <param name="document">Загруженный документ из файла</param>
    /// <param name="order">Заказ</param>
    /// <returns></returns>
    public virtual Document PrepareDocument(XWPFDocument document, Order order)
    {
      var replacePhrases = new[]
      {
        new Tuple<string, string>("#Дата_Передачи", DateTime.Now.ToString("D")),
        new Tuple<string, string>("#Номер_Заказа", order.Id.ToString()),
        new Tuple<string, string>("#Статус_Заказа",
          _converter.Convert(order.Status, typeof(string), null, CultureInfo.CurrentCulture)?.ToString())
      };

      foreach (var paragraph in document.Paragraphs)
      {
        foreach (var replacePhrase in replacePhrases)
        {
          try
          {
            paragraph.ReplaceText(replacePhrase.Item1, replacePhrase.Item2);
          }
          catch (Exception e)
          {
          }
        }
      }


      var documentTables = CheckTables(document);

      foreach (var documentTable in documentTables)
      {
        var rowTemplate = documentTable.GetRow(1);
      
        foreach (var orderInstance in order.Instances)
        {
          var row = documentTable.CreateRow();
        
          row.GetCell(0).SetText(orderInstance.TagNumber.ToString());
          row.GetCell(1).SetText(orderInstance.ClothKindObj.Name);
          row.GetCell(2).SetText(
            _measureKindConverter
              .Convert(orderInstance.ClothKindObj.MeasureKind, typeof(string), null, CultureInfo.CurrentCulture)
              ?.ToString());
          row.GetCell(3).SetText(orderInstance.Amount.ToString());
          row.GetCell(4).SetText(orderInstance.Comment ?? string.Empty);

          documentTable.AddRow(row);
        }

        documentTable.RemoveRow(1);
      }

      return document;
    }

    private List<XWPFTable> CheckTables(XWPFDocument document)
    {
      var matchingTables = document.Tables.Where(x =>
      {
        var row = x.GetRow(1);
        return
          row.GetCell(0).GetText() == "#№" &&
          row.GetCell(1).GetText() == "#Наименование" &&
          row.GetCell(2).GetText() == "#Ед_Изм" &&
          row.GetCell(3).GetText() == "#Кол-во" &&
          row.GetCell(4).GetText() == "#Комментарий";
      }).ToList();

      return matchingTables;
    }

    private void WriteDocumentation(Order order)
    {
      XWPFDocument document = null;
      try
      {
        using (FileStream file = new FileStream($"Resources/{_documentName}", FileMode.Open, FileAccess.Read))
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