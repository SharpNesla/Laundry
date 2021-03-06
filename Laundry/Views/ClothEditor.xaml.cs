﻿using System;
using System.Collections.Generic;
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
using Laundry.Utils;
using Laundry.Utils.Controls.EntitySearchControls;
using MaterialDesignThemes.Wpf;
using Action = System.Action;

namespace Laundry.Views
{
  /// <summary>
  /// Interaction logic for ClothEditor.xaml
  /// </summary>
  public class ClothEditorViewModel : EditorScreen<Repository<ClothInstance>, ClothInstance>
  {
    private int _instancePos;
    public Order Order { get; set; }
    public bool IsNewOrder { get; set; }

    public ClothKindSelectorViewModel ClothKindTree { get; set; }

    public event Action Changed;

    public ClothEditorViewModel(IEventAggregator aggregator, IModel model, Order order) :
      base(aggregator, model, null, "предмета одежды")
    {
      this.Order = order;
      this.ClothKindTree = new ClothKindSelectorViewModel(model);

      this.ClothKindTree.SelectedEntity = model.ClothKinds.GetById(0);

      this.ClothKindTree.EntityChanged = x => { this.Entity.ClothKind = x.Id;
        this.Entity.ClothKindObj = x;
      };

      this.Entity.ClothKindObj = ClothKindTree.SelectedEntity;
    }

    public override void ApplyChanges()
    {
      if (IsNew)
      {
        this.Order.Instances.Add(this.Entity);
      }
      else
      {
        this.Order.Instances[_instancePos] = this.Entity;
      }

      DialogHostExtensions.CloseCurrent();

      Changed?.Invoke();
    }
  
    public override void Handle(ClothInstance message)
    {
      this.Entity = message.Clone();
      this._instancePos = this.Order.Instances.IndexOf(message);
      this.ClothKindTree.SelectedEntity = this.Entity.ClothKindObj;
      this.IsNew = false;
    }
  }
}