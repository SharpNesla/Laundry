﻿using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Caliburn.Micro;
using Model;
using Model.CollectionRepositories;
using Model.DatabaseClients;
using PropertyChanged;

namespace Laundry.Utils
{
  /// <summary>
  /// Базовый класс экрана-редактора сущности
  /// </summary>
  /// <typeparam name="TRepository">Тип repository сущнсоти</typeparam>
  /// <typeparam name="TEntity">Тип сущности</typeparam>
  public class EditorScreen<TRepository, TEntity> : ActivityScreen, IHandle<TEntity>
    where TEntity : RepositoryElement, new()
    where TRepository : Repository<TEntity>
  {
    [AlsoNotifyFor(nameof(EditorTitle))]
    public bool IsNew { get; set; }

    /// <summary>
    /// Создаваемая или редактируемая сущность
    /// </summary>
    [DoNotNotify]
    public TEntity Entity { get; set; }

    /// <summary>
    /// Динамический заголовок окна, включающий в себя Id
    /// </summary>
    public virtual string EditorTitle
    {
      get { return !IsNew ? $"Редактирование {EntityName} №{Entity.Id}" : $"Редактирование нового {EntityName}"; }
    }

    protected readonly string EntityName;

    /// <summary>
    /// Repository с сущностью
    /// </summary>
    protected TRepository EntityRepository { get; set; }

    public EditorScreen(IEventAggregator aggregator, IModel model, TRepository entityRepo,
      string entityTitleName = "объекта") : base(aggregator, model)
    {
      this.EventAggregator.Subscribe(this);
      this.EntityRepository = entityRepo;

      this.EntityName = entityTitleName;

      this.IsNew = true;
      this.Entity = new TEntity();
    }

    /// <summary>
    /// Отмена и переход на предыдущий экран
    /// </summary>
    public virtual void Discard()
    {
      ChangeApplicationScreen(Screens.Context);
    }

    /// <inheritdoc />
    public virtual void Handle(TEntity message)
    {
      this.Entity = this.EntityRepository.GetById(message.Id);
      this.IsNew = false;
      this.EventAggregator.Unsubscribe(this);
    }

    /// <summary>
    /// Применить изменения или добавить сущность
    /// в базу данных
    /// </summary>
    public virtual void ApplyChanges()
    {
      if (IsNew)
      {
        EntityRepository.Add(this.Entity);
      }
      else
      {
        EntityRepository.Update(this.Entity);
      }

      ChangeApplicationScreen(Screens.Context);
    }

    /// <summary>
    /// Производит UpdateSource всех textbox и combobox
    /// в передаваемой view для обновления валидаций на полях
    /// и проверяет их на валидность
    /// </summary>
    /// <param name="view"></param>
    public virtual void ApplyChanges(DependencyObject view)
    {
      var tree = FindVisualChildren<TextBox>(view);
      var comboboxes = FindVisualChildren<ComboBox>(view).Where(x=>x.IsEnabled);
      foreach (TextBox tb in tree)
      {
        tb.GetBindingExpression(TextBox.TextProperty)?.UpdateSource();
      }

      foreach (ComboBox tb in comboboxes)
      {
        tb.GetBindingExpression(ComboBox.SelectedItemProperty)?.UpdateSource();
      }


      foreach (TextBox tb in tree)
      {
        if (Validation.GetHasError(tb))
        {
          return;
        }
      }

      foreach (var cb in comboboxes)
      {
        if (Validation.GetHasError(cb))
        {
          return;
        }
      }

      this.ApplyChanges();
    }

    /// <summary>
    /// Метод, позволяющий получить всех детей данного
    /// элемента в визуальном дереве элементов View
    /// </summary>
    /// <typeparam name="T">Тип отбираемого контрола</typeparam>
    /// <param name="depObj">View или её элемент</param>
    /// <returns>Перечислимое всех детей данного элемент</returns>
    public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
    {
      if (depObj != null)
      {
        for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
        {
          DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
          if (child != null && child is T)
          {
            yield return (T) child;
          }

          foreach (T childOfChild in FindVisualChildren<T>(child))
          {
            yield return childOfChild;
          }
        }
      }
    }
  }
}