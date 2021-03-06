﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using Model;
using Model.CollectionRepositories;
using Model.DatabaseClients;
using Laundry.Utils.Controls;

namespace Laundry.Utils
{
  /// <summary>
  /// Базовый класс справочника сущности
  /// </summary>
  /// <typeparam name="TGrid">Тип таблицы сущности</typeparam>
  public class DictionaryScreen<TGrid> : DrawerActivityScreen
    where TGrid : IEntityGrid<RepositoryElement>
  {
    public PaginatorViewModel Paginator { get; set; }
    public TGrid EntityGrid { get; set; }

    public DictionaryScreen(IEventAggregator aggregator, IModel model, PaginatorViewModel paginator,
      TGrid entityGrid, string paginatorLabel = "Объектов") :
      base(aggregator, model)
    {
      this.EntityGrid = entityGrid;

      this.Paginator = paginator;
      this.Paginator.ElementsName = paginatorLabel;
      this.Paginator.RegisterPaginable(entityGrid, false);
    }

    protected override void OnActivate()
    {
      base.OnActivate();
      this.Paginator.RefreshPaginable();
    }

    public void AddEntity()
    {
      this.EntityGrid.Add();
    }

    public void AdvancedSearch()
    {
      this.Paginator.RefreshPaginable();
    }

    public void ExportToExcel()
    {
      EntityGrid.ExportToExcel();
    }

    public void ExportToCSV()
    {
      EntityGrid.ExportToCSV();
    }

    public void RemoveSelectedGroup()
    {
      EntityGrid.RemoveSelectedGroup();
    }
  }
}