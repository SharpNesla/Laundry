using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Caliburn.Micro;
using Model;
using Model.CollectionRepositories;
using Laundry.Utils;
using Laundry.Utils.Controls;
using Laundry.Utils.Controls.EntitySearchControls;
using Laundry.Utils.Converters;
using MongoDB.Driver;
using NPOI.OpenXmlFormats.Wordprocessing;
using NPOI.XWPF.UserModel;
using PropertyChanged;

namespace Laundry.Views
{
  /// <inheritdoc cref="EditorScreen{TRepository, TEntity}" />
  /// <summary>
  /// View-модель редактирования заказа
  /// </summary>
  public class OrderEditorViewModel : EditorScreen<OrderRepository, Order>, IHandle<Client>
  {
    /// <inheritdoc />
    /// <summary>
    /// Переопределение заголовка редактора (включает дату создания)
    /// </summary>
    public override string EditorTitle
    {
      get
      {
        return !IsNew
          ? $"Редактирование {EntityName} №{Entity.Id} от {Entity.CreationDate:dd.MM.yyyy, HH:mm}"
          : $"Редактирование нового {EntityName} от от {Entity.CreationDate:dd.MM.yyyy, HH:mm}";
      }
    }

    public double Price => this.Model.Orders.CalculatePrice(this.Entity);

    public bool IsDiscount
    {
      get { return this.Entity.IsDiscount; }
      set
      {
        this.Entity.IsDiscount = value;
        this.NotifyOfPropertyChange();
        this.NotifyOfPropertyChange(nameof(Price));
      }
    }

    public bool IsCustomPrice
    {
      get { return this.Entity.IsCustomPrice; }
      set
      {
        this.Entity.IsCustomPrice = value;

        this.NotifyOfPropertyChange();
        this.NotifyOfPropertyChange(nameof(Price));
      }
    }

    public ClientSearchViewModel ClientCombo { get; set; }
    public ClothDataGridViewModel ClothInstancesGrid { get; set; }
    public PaginatorViewModel Paginator { get; set; }

    #region SearchView-ы работников для создания связей с заказом

    public EmployeeSearchViewModel ObtainerCombo { get; set; }
    public ClientSearchViewModel CorpObtainerCombo { get; set; }
    public EmployeeSearchViewModel InCourierCombo { get; set; }
    public EmployeeSearchViewModel WasherCombo { get; set; }
    public EmployeeSearchViewModel OutCourierCombo { get; set; }
    public EmployeeSearchViewModel DistributerCombo { get; set; }
    public ClientSearchViewModel CorpDistributerCombo { get; set; }

    #endregion

    public OrderEditorViewModel(IEventAggregator aggregator, IModel model, ClothDataGridViewModel clothGrid,
      PaginatorViewModel paginator)
      : base(aggregator, model, model.Orders, "заказа")
    {
      aggregator.Subscribe(this);

      this.Entity.CreationDate = DateTime.Now;
      this.Entity.ExecutionDate = DateTime.Now.AddDays(1);

      this.ClothInstancesGrid = clothGrid;
      this.ClothInstancesGrid.Order = this.Entity;
      this.ClothInstancesGrid.StateChanged += () => NotifyOfPropertyChange(nameof(Price));
      this.Paginator = paginator;
      this.Paginator.ElementsName = "Вещей";
      this.Paginator.RegisterPaginable(this.ClothInstancesGrid);

      this.ClientCombo = new ClientSearchViewModel(model);
      this.ClientCombo.EntityChanged += OnEntityChanged;
    }

    protected override void OnActivate()
    {
      base.OnActivate();

      #region Инициализация поисковых комбобоксов

      this.ObtainerCombo = new EmployeeSearchViewModel(Model, "Приёмщик",
        Builders<Employee>.Filter.Eq(nameof(Employee.Profession), EmployeeProfession.Advisor));
      this.CorpObtainerCombo = new ClientSearchViewModel(Model, "Приёмщик (корпоративный)",
        Builders<Client>.Filter.Eq(nameof(Client.IsCorporative), true));
      this.InCourierCombo = new EmployeeSearchViewModel(Model, "Курьер, забирающий заказ",
        Builders<Employee>.Filter.Eq(nameof(Employee.Profession), EmployeeProfession.Courier));
      this.WasherCombo = new EmployeeSearchViewModel(Model, "Прачечник",
        Builders<Employee>.Filter.Eq(nameof(Employee.Profession), EmployeeProfession.Washer));
      this.OutCourierCombo = new EmployeeSearchViewModel(Model, "Курьер, вовзращающий заказ",
        Builders<Employee>.Filter.Eq(nameof(Employee.Profession), EmployeeProfession.Courier));
      this.CorpDistributerCombo = new ClientSearchViewModel(Model, "Приёмщик (корпоративный), принимающий заказ",
        Builders<Client>.Filter.Eq(nameof(Client.IsCorporative), true));
      this.DistributerCombo = new EmployeeSearchViewModel(Model, "Приёмщик, выдающий заказ",
        Builders<Employee>.Filter.Eq(nameof(Employee.Profession), EmployeeProfession.Advisor));

      this.ObtainerCombo.EntityChanged += obtainer => this.EntityRepository.SetObtainer(this.Entity, obtainer);
      this.CorpObtainerCombo.EntityChanged +=
        corpObtainer => this.EntityRepository.SetObtainer(this.Entity, corpObtainer);
      this.InCourierCombo.EntityChanged += inCourier => this.EntityRepository.SetInCourier(this.Entity, inCourier);
      this.WasherCombo.EntityChanged += washer => this.EntityRepository.SetWasher(this.Entity, washer);
      this.OutCourierCombo.EntityChanged += outCourier => this.EntityRepository.SetOutCourier(this.Entity, outCourier);
      this.CorpDistributerCombo.EntityChanged += corpDistributer =>
        this.EntityRepository.SetDistributer(this.Entity, corpDistributer);
      this.DistributerCombo.EntityChanged +=
        distributer => this.EntityRepository.SetDistributer(this.Entity, distributer);

      #endregion
    }

    /// <summary>
    /// Добавить одежду
    /// </summary>
    public void AddCloth()
    {
      this.ClothInstancesGrid.Add();
    }

    private void OnEntityChanged(Client obj)
    {
      this.EntityRepository.SetClient(this.Entity, obj);
      this.NotifyOfPropertyChange(nameof(Price));
    }

    /// <inheritdoc />
    public override void Handle(Order message)
    {
      base.Handle(message);

      this.Entity = this.EntityRepository.GetById(message.Id);

      this.ClothInstancesGrid.Order = this.Entity;
      this.ClothInstancesGrid.Refresh(0, int.MaxValue);
      this.ClientCombo.SelectedEntity = EntityRepository.GetClient(this.Entity);
      if (message.IsCorporative)
      {
        CorpObtainerCombo.SelectedEntity = Model.Clients.GetById(message.CorpObtainerId);
        CorpDistributerCombo.SelectedEntity = Model.Clients.GetById(message.CorpDistributerId);
      }
      else
      {
        ObtainerCombo.SelectedEntity = Model.Employees.GetById(message.ObtainerId);
        DistributerCombo.SelectedEntity = Model.Employees.GetById(message.DistributerId);
      }

      InCourierCombo.SelectedEntity = Model.Employees.GetById(message.InCourierId);

      WasherCombo.SelectedEntity = Model.Employees.GetById(message.WasherCourierId);

      OutCourierCombo.SelectedEntity = Model.Employees.GetById(message.OutCourierId);
    }

    /// <summary>
    /// Обработчик посыла клиента в редактор
    /// </summary>
    /// <param name="message"></param>
    public void Handle(Client message)
    {
      this.ClientCombo.SelectedEntity = Model.Clients.GetById(message.Id);
    }

    private readonly OrderStatusConverter _converter = new OrderStatusConverter();
    private readonly MeasureKindConverter _measureKindConverter = new MeasureKindConverter();

    protected virtual IEnumerable<Tuple<string, string>> PrepareReplaceText(Order order)
    {
      if (order == null)
      {
        order = this.Entity;
      }

      var client = Model.Clients.GetById(order.ClientId);

      return new[]
      {
        new Tuple<string, string>("#Дата_Передачи", DateTime.Now.ToString("D")),
        new Tuple<string, string>("#Дата_Выдачи", DateTime.Now.ToString("D")),
        new Tuple<string, string>("#Дата_Приёма", order.CreationDate.ToString("D")),
        new Tuple<string, string>("#Дата_Исполнения", order.ExecutionDate.ToString("D")),

        new Tuple<string, string>("#Цена_Заказа", $"{order.Price}₽"),
        new Tuple<string, string>("#Номер_Заказа", order.Id.ToString()),
        new Tuple<string, string>("#Статус_Заказа",
          _converter.Convert(order.Status, typeof(string), null, CultureInfo.CurrentCulture)?.ToString()),

        new Tuple<string, string>("#ФИО_Клиента", client.ToString()),
        new Tuple<string, string>("#Номер_Телефона_Клиента", client.PhoneNumber),
        new Tuple<string, string>("#ФИО_Выдающего_Приёмщика",
          Entity.IsCorporative
            ? Model.Clients.GetById(order.CorpDistributerId).ToString()
            : Model.Employees.GetById(order.DistributerId).ToString()),
        new Tuple<string, string>("#Адрес_Клиента", $"Г. {client.City}, ул. {client.Street}," +
                                                    $" д. {client.House}, кв. {client.Flat}, почтовый индекс {client.ZipCode}")
      };
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="document"></param>
    /// <param name="order"></param>
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
        var rowTemplate = documentTable.GetRow(1);

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
        try
        {
          var row = x.GetRow(1);
          return
            row.GetCell(0).Paragraphs[0].Text == "#№" &&
            row.GetCell(1).Paragraphs[0].Text == "#Наименование" &&
            row.GetCell(2).Paragraphs[0].Text == "#Ед_Изм" &&
            row.GetCell(3).Paragraphs[0].Text == "#Кол-во" &&
            row.GetCell(4).Paragraphs[0].Text == "#Комментарий";
        }
        catch (NullReferenceException)
        {
          return false;
        }
      }).ToList();

      return matchingTables;
    }

    /// <summary>
    /// Экспортировать документ (квитанцию или договор)
    /// </summary>
    public void WriteDocumentation()
    {
      var documentName = Entity.IsCorporative ? "Contract.docx" : "ObtainCheck.docx";
      XWPFDocument document;
      try
      {
        using (FileStream file = new FileStream($"Resources/{documentName}", FileMode.Open, FileAccess.Read))
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
        PrepareDocument(document, this.Entity);

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