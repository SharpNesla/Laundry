using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using AutoDependencyPropertyMarker;
using Caliburn.Micro;
using Laundry.Model;
using PropertyChanged;

namespace Laundry.Utils.Controls
{
  /// <summary>
  /// Interaction logic for Paginator.xaml
  /// </summary>

  [AddINotifyPropertyChangedInterface]
  public class PaginatorViewModel
  {

    private void CheckButtons(object sender, PropertyChangedEventArgs e)
    {
      CheckButtons();
    }

    public string ElementsName { get; set; }
    public int ElementsPerPage { get; set; }

    public int[] ComboValues { get; }

    public PaginatorViewModel()
    {
      this.ComboValues = new int[] {5, 10, 20, 50, 100};
      this.ElementsPerPage = 5;
    }

    public bool IsMoveNextEnabled { get; private set; }
    public bool IsMovePreviousEnabled { get; private set; }
    


    public void MoveNext()
    {
      this.CheckButtons();
    }

    public void ChangeElementsPerPage()
    {
     
      this.CheckButtons();
    }

    public void MovePrevious()
    {
      //this.PagingCollectionView.MoveToPreviousPage();
      this.CheckButtons();
    }

    private void CheckButtons()
    {
      //this.IsMovePreviousEnabled = this.PagingCollectionView.CurrentPage != 1;
      //this.IsMoveNextEnabled = this.PagingCollectionView.CurrentPage != this.PagingCollectionView.PageCount;
    }
  }
}
