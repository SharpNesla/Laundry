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
    protected IModel Model;

    protected OrderActionsBase(OrderRepository orderRepo, IModel model, string orderEmployeeInvolvement,
      OrderDataGridViewModel orderGrid,
      OrderStatus startStatus, OrderStatus changingStatus, string documentName = null,
      FilterDefinition<Order> additionalFilter = null)
    {
      this.Model = model;
      Repository = orderRepo;
      _changingStatus = changingStatus;
      _documentName = documentName;
      OrderGrid = orderGrid;
      OrderGrid.Filter =
        Builders<Order>.Filter.And(
          Builders<Order>.Filter.Eq(nameof(Order.Status), startStatus),
          Builders<Order>.Filter.Eq(orderEmployeeInvolvement, this.Model.CurrentUser.Id),
          additionalFilter ?? Builders<Order>.Filter.Empty);
      this.OrderGrid.Refresh(0, int.MaxValue);
    }

    protected virtual IEnumerable<Tuple<string, string>> PrepareReplaceText(Order order)
    {
      return new[]
      {
        new Tuple<string, string>("#Дата_Передачи", DateTime.Now.ToString("f")),
        new Tuple<string, string>("#Дата_Выдачи", DateTime.Now.ToString("f")),
        new Tuple<string, string>("#Дата_Приёма", order.CreationDate.ToString("f")),
        new Tuple<string, string>("#Дата_Исполнения", order.ExecutionDate.ToString("f")),

        new Tuple<string, string>("#Номер_Заказа", order.Id.ToString()),
        new Tuple<string, string>("#Статус_Заказа",
          _converter.Convert(order.Status, typeof(string), null, CultureInfo.CurrentCulture)?.ToString()),

        new Tuple<string, string>("#ФИО_Клиента", Model.Clients.GetById(order.ClientId).ToString()),
        new Tuple<string, string>("#ФИО_Выдающего_Приёмщика", Model.Employees.GetById(order.DistributerId).ToString()),
        new Tuple<string, string>("#ФИО_Прачечника", Model.Employees.GetById(order.WasherCourierId).ToString()),
      };
    }

    public virtual void Apply()
    {
      this.Repository.SetOrdersStatus(this.OrderGrid.SelectedEntities, _changingStatus);
    }

    public void WriteDocumentation()
    {
      if (string.IsNullOrEmpty(_documentName))
      {
        return;
      }

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
      foreach (var paragraph in document.Paragraphs)
      {
        foreach (var replacePhrase in this.PrepareReplaceText(order))
        {
          //catch на случай, если искомая фраза не была найдена
          try
          {
            paragraph.ReplaceText(replacePhrase.Item1, replacePhrase.Item2);
          }
          catch (Exception)
          {
          }
        }
      }


      var documentTables = CheckTables(document);

      foreach (var documentTable in documentTables)
      {
        foreach (var orderInstance in order.Instances)
        {
          try
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
          }
          catch (NullReferenceException)
          {
            break;
          }
        }

        documentTable.RemoveRow(1);
      }

      return document;
    }

    /// <summary>
    /// Проверить документ на содержание в нём подходящей таблицы для
    /// вставки информации об экземплярах одежды
    /// </summary>
    /// <param name="document">Документ</param>
    /// <returns>Лист с таблицами, прошедшими проверку</returns>
    private List<XWPFTable> CheckTables(XWPFDocument document)
    {
      var matchingTables = document.Tables.Where(x =>
      {
        var row = x.GetRow(1);
        return
          row.GetCell(0).Paragraphs[0].Text == "#№" &&
          row.GetCell(1).Paragraphs[0].Text == "#Наименование" &&
          row.GetCell(2).Paragraphs[0].Text == "#Ед_Изм" &&
          row.GetCell(3).Paragraphs[0].Text == "#Кол-во" &&
          row.GetCell(4).Paragraphs[0].Text == "#Комментарий";
      }).ToList();

      return matchingTables;
    }

    private void WriteDocumentation(Order order)
    {
      XWPFDocument document;
      try
      {
        using (FileStream file = new FileStream($"Resources/{_documentName}", FileMode.Open, FileAccess.Read))
        {
          document = new XWPFDocument(file);
        }
      }
      catch (Exception)
      {
        return;
      }

      if (document != null)
      {
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
            PrepareDocument(document, order);
            document.Write(fs);
          }
        }
      }
    }
  }
}