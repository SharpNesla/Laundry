﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Caliburn.Micro;
using Model;
using Model.CollectionRepositories;
using Laundry.Views;
using Laundry.Views.Cards;
using MongoDB.Driver;
using NPOI.XSSF.UserModel;
using PropertyChanged;

namespace Laundry.Utils.Controls
{
  public class ClothDataGridViewModel : EntityGrid<ClothInstance, ClothInstancesRepository, ClothInstanceCardViewModel>
  {

    //public override FilterDefinition<Client> Filter
    //{
    //  get
    //  {
    //    var filter = BaseFilter;

    //    if (this.I)
    //    {
    //      filter = Builders<Client>.Filter.And(
    //        this.BaseFilter,
    //        Builders<Client>.Filter.Gte(nameof(Client.DateBirth), this.LowDateBirthBound ?? DateTime.MinValue),
    //        Builders<Client>.Filter.Lte(nameof(Client.DateBirth), this.HighDateBirthBound ?? DateTime.MaxValue));
    //    }

    //    return filter;
    //  }

    //  set { base.Filter = value; }
    //}

    private readonly IEventAggregator _eventAggregator;
    private readonly IModel _model;
    public Order Order { get; set; }
    
    public ClothDataGridViewModel(IEventAggregator eventAggregator,
      ClothInstanceCardViewModel card, IModel model,
      DeleteDialogViewModel shure) : base(eventAggregator, card, model.ClothInstances, shure, Screens.About)
    {
      _eventAggregator = eventAggregator;
      _model = model;
    }

    public override async void Add()
    {
      var editor = new ClothEditorViewModel(_eventAggregator, _model, this.Order);
      await DialogHostExtensions.ShowCaliburnVM(editor);
      RaiseStateChanged();
    }

    public override async void Edit()
    {
      var editor = new ClothEditorViewModel(_eventAggregator, _model, this.Order);

      _eventAggregator.PublishOnUIThread(SelectedEntity);
      await DialogHostExtensions.ShowCaliburnVM(editor);
      RaiseStateChanged();
    }

    public override void Refresh(int page, int elements)
    {
      this.Entities = Order.Instances.AsReadOnly();
    }

    public override long Count => Order.Instances.Count;

    protected override XSSFWorkbook PrepareWorkBook(XSSFWorkbook workbook)
    {
      throw new NotImplementedException();
    }

  }
}