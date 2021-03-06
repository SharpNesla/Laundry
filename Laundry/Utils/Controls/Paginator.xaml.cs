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
using Model;
using PropertyChanged;
using Action = System.Action;

namespace Laundry.Utils.Controls
{
  /// <summary>
  /// ������������ ����������� ��������
  /// </summary>
  public interface IPaginable
  {
    /// <summary>
    /// ����� ���������� ��������� � ������������ �������
    /// </summary>
    long Count { get; }

    /// <summary>
    /// ���������� �������� �� ���������� �������� � ���������� ���������
    /// </summary>
    /// <param name="page">��������</param>
    /// <param name="elements">���������� ��������� �� ��������</param>
    void Refresh(int page, int elements);

    /// <summary>
    /// ���������� �������, ���������� �� ��������
    ///  ��� ��������� ��������� ��� (��� ������� CRUD-��������)
    /// </summary>
    event Action StateChanged;
  }

  /// <inheritdoc />
  /// <summary>
  /// View-model ����������, ���������� ����������� ����� IPaginable
  /// </summary>
  public class PaginatorViewModel : PropertyChangedBase
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
      }
    }

    public int MaxPages
    {
      get
      {
        if (Count != 0)
        {
          return (int) Math.Ceiling((double) Count / ElementsPerPage);
        }

        return 1;
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
      this.ElementsPerPage = 10;
      this.CurrentPage = 1;
      CheckButtons();
    }

    public bool IsMoveNextEnabled { get; private set; }
    public bool IsMovePreviousEnabled { get; private set; }

    public event Action<int, int> Changed;

    /// <summary>
    /// ����������� �� �������� �����
    /// </summary>
    public void MoveNext()
    {
      this.CurrentPage++;
    }

    public void ChangeElementsPerPage()
    {
      this.CurrentPage = 1;
    }

    /// <summary>
    /// ����������� �� �������� �����
    /// </summary>
    public void MovePrevious()
    {
      this.CurrentPage--;
    }

    /// <summary>
    /// ��������� ������ � ������������� � ������
    /// ���������� �� ������ ��� ��������� ��������
    /// </summary>
    private void CheckButtons()
    {
      IsMovePreviousEnabled = CurrentPage != 1;
      IsMoveNextEnabled = CurrentPage != MaxPages;
    }

    /// <summary>
    /// ��������� ������������ �������� (IPaginable)
    /// � ����������
    /// </summary>
    /// <param name="paginable">������������ ��������</param>
    /// <param name="refreshOnRegister">���� </param>
    public void RegisterPaginable(IPaginable paginable, bool refreshOnRegister = true)
    {
      this.ClearPaginable();
      this.Paginable = paginable;

      this.Paginable.StateChanged += RefreshPaginable;
      this.Changed += Paginable.Refresh;
      if (refreshOnRegister)
      {
        RefreshPaginable();
      }
    }

    public void ClearPaginable()
    {
      var paginable = this.Paginable;
      if (paginable != null) paginable.StateChanged -= RefreshPaginable;
      this.Paginable = null;
    }

    /// <summary>
    /// �������� ������������ �������� �� ������� ��������
    /// </summary>
    public void RefreshPaginable()
    {
      this.Count = Paginable.Count;
      if (this.CurrentPage > this.MaxPages)
      {
        this.CurrentPage = 1;
      }

      Paginable.Refresh(this.CurrentPage - 1, this.ElementsPerPage);
    }

    public IPaginable Paginable { get; private set; }
  }
}