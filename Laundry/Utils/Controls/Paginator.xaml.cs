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
using Action = System.Action;

namespace Laundry.Utils.Controls
{
  /// <summary>
  /// Interaction logic for Paginator.xaml
  /// </summary>

  [AddINotifyPropertyChangedInterface]
  public class PaginatorViewModel
  {
    private int _currentPage;
    private long _count;
    private int _elementsPerPage;

    public string ElementsName { get; set; }

    public int ElementsPerPage
    {
      get { return _elementsPerPage; }
      set
      {
        _elementsPerPage = value;
        CheckButtons();
        this.Changed?.Invoke(this.CurrentPage - 1, this.ElementsPerPage);
      }
    }

    public long Count
    {
      get { return _count; }
      set
      {
        _count = value;
        CheckButtons();
        this.Changed?.Invoke(this.CurrentPage - 1, this.ElementsPerPage);
      }
    }

    public int MaxPages
    {
      get
      {
        if (ElementsPerPage != 0)
        {
          return (int) Math.Ceiling((double)Count / ElementsPerPage);
        }
        else
        {
          return 1;
        }
      }
    }

    public int CurrentPage
    {
      get { return _currentPage; }
      set
      {
        _currentPage = value;
        CheckButtons();
        this.Changed?.Invoke(this.CurrentPage - 1, this.ElementsPerPage);
      }
    }

    public int[] ComboValues { get; }

    public PaginatorViewModel()
    {
      this.ComboValues = new int[] {5, 10, 20, 50, 100};
      this.ElementsPerPage = 5;
      this.CurrentPage = 1;
      CheckButtons();
    }

    public bool IsMoveNextEnabled { get; private set; }
    public bool IsMovePreviousEnabled { get; private set; }
    
    public event Action<int,int> Changed;

    public void MoveNext()
    {
      this.CurrentPage++;
    }

    public void ChangeElementsPerPage()
    {
      this.CurrentPage = 1;
    }

    public void MovePrevious()
    {
      this.CurrentPage--;
    }

    private void CheckButtons()
    { 
      IsMovePreviousEnabled = CurrentPage != 1;
      IsMoveNextEnabled = CurrentPage != MaxPages;
    }
  }
}
