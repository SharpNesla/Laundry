﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Caliburn.Micro;
using Laundry.Model;
using Laundry.Model.CollectionRepositories;
using Laundry.Views;
using Laundry.Views.Cards;
using LiveCharts;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace Laundry.Utils.Controls
{
  /// <summary>
  /// Interaction logic for ClothKindGrid.xaml
  /// </summary>
  public class ClothKindGridViewModel : EntityGrid<ClothKind, ClothKindRepository, ClothKindCardViewModel>, IChartable<ClothKind>
  {
    private readonly IModel _model;
    public float NameWidth { get; set; }
    public ObservableCollection<ClothKind> EditableEntities { get; private set; }


    public ClothKindGridViewModel(IEventAggregator eventAggregator, ClothKindCardViewModel card,
      IModel model, DeleteDialogViewModel shure)
      : base(eventAggregator, card, model.ClothKinds, shure, Screens.ClothKindEditor)
    {
      _model = model;
      this.EditableEntities = new ObservableCollection<ClothKind> {Repo.GetById(0)};
      this.StateChanged += RaiseStateChanged; 
    }

    public override async void Add()
    {
      var editor = new ClothKindEditorViewModel(this.EventAggregator, _model);
      editor.ClothKindParent = this.SelectedEntity;
      await DialogHostExtensions.ShowCaliburnVM(editor);
      this.Refresh(0, 0);
    }
    public override async void Edit()
    {
      var editor = new ClothKindEditorViewModel(this.EventAggregator, _model);
      EventAggregator.PublishOnUIThread(this.SelectedEntity);
      await DialogHostExtensions.ShowCaliburnVM(editor);
      this.Refresh(0, 0);
    }

    public void ShowHideDetails(ToggleButton button, ClothKind clothKind, ClothKindTreeView view)
    {
      if (button.IsChecked.Value)
      {
        this.Repo.FetchChildren(clothKind);

        foreach (var kind in clothKind.Children)
        {
          kind.Level = clothKind.Level + 1;
          this.EditableEntities.Insert(EditableEntities.IndexOf(clothKind) + 1, kind);
        }

        //view.MainGrid.Columns[0].Width = new DataGridLength((clothKind.Level + 1) * 64, DataGridLengthUnitType.Pixel);
      }

      else
      {
        RemoveChildren(clothKind);

        //view.MainGrid.Columns[0].Width = new DataGridLength((clothKind.Level) * 64 + 64, DataGridLengthUnitType.Pixel);
      }
    }

    public void RemoveChildren(ClothKind clothKind)
    {
      if (clothKind.HasChildren && clothKind.Children != null)
      {
        foreach (var kind in clothKind.Children)
        {
          this.EditableEntities.Remove(kind);
          RemoveChildren(kind);
        }
      }
    }

    public override void Refresh(int page, int elements)
    {
      this.EditableEntities = new ObservableCollection<ClothKind> { Repo.GetById(0) };
    }
    
    public SeriesCollection Values { get; }
    public string[] Labels { get; }
    public string LabelsTitle => "Виды одежды";
    public string ValuesTitle => "Цена";
  }
}