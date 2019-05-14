using System.Collections.Generic;
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
  public class EditorScreen<TRepository, TEntity> : ActivityScreen, IHandle<TEntity>
    where TEntity : class, IRepositoryElement, new()
    where TRepository : Repository<TEntity>
  {
    [AlsoNotifyFor(nameof(EditorTitle))]
    public bool IsNew { get; set; }

    [DoNotNotify]
    public TEntity Entity { get; set; }

    public virtual string EditorTitle
    {
      get { return !IsNew ? $"Редактирование {EntityName} №{Entity.Id}" : $"Редактирование нового {EntityName}"; }
    }

    protected readonly string EntityName;

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

    public virtual void Discard()
    {
      ChangeApplicationScreen(Screens.Context);
    }

    public virtual void Handle(TEntity message)
    {
      this.Entity = this.EntityRepository.GetById(message.Id);
      this.IsNew = false;
      this.EventAggregator.Unsubscribe(this);
    }

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

    public virtual void ApplyChanges(DependencyObject view)
    {
      var tree = FindVisualChildren<TextBox>(view);
      var comboboxes = FindVisualChildren<ComboBox>(view);
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