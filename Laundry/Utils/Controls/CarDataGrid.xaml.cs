using System;
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
using Laundry.Views;
using Laundry.Views.Cards;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace Laundry.Utils.Controls
{
  /// <summary>
  /// Interaction logic for CarDataGrid.xaml
  /// </summary>
  public class CarDataGridViewModel : EntityGrid<Car, Repository<Car>, CarCardViewModel>
  {
    public CarDataGridViewModel(IEventAggregator eventAggregator, CarCardViewModel card, DeleteDialogViewModel deleteDialog, IModel model) 
      : base(eventAggregator, card, model.Cars,deleteDialog, Screens.CarEditor)
    {
    }

    public override string[] TableSheetHeader => new []{"№", "Марка и модель", "VIN", "Цвет", "Номерной знак", "Год выпуска", "Комментарий"};

    public override string TableSheetName => "Автомобили";

    protected override IRow PrepareEntityRow(ISheet sheet, Car entity)
    {
      var row = sheet.CreateRow(sheet.PhysicalNumberOfRows);

      row.CreateCell(0).SetCellValue(entity.Id);
      row.CreateCell(1).SetCellValue(entity.BrandAndModel);
      row.CreateCell(2).SetCellValue(entity.VIN);
      row.CreateCell(3).SetCellValue(entity.Color);
      row.CreateCell(4).SetCellValue(entity.Sign);
      row.CreateCell(5).SetCellValue(entity.CreationYear ?? 0);
      row.CreateCell(6).SetCellValue(entity.Comment);
      
      return row;
    }


  }
}
