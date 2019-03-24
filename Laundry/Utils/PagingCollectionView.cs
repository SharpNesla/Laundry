using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;

namespace Laundry.Utils
{
  public class PagingCollectionView : CollectionView
  {
    private int _currentPage = 1;

    public PagingCollectionView(IEnumerable innerList, int itemsPerPage)
      : base(innerList)
    {
      this.ItemsPerPage = itemsPerPage;
    }

    public int FilteredCount
    {
      get { return FilteredCollection.Count(); }
    }

    private IEnumerable<object> FilteredCollection => this.SourceCollection.OfType<object>().Where(o => Filter(o));

    public override int Count
    {
      get
      {
        if (FilteredCount == 0) return 0;
        if (this._currentPage < this.PageCount) // page 1..n-1
        {
          return this.ItemsPerPage;
        }
        else // page n
        {
          var itemsLeft = FilteredCount % this.ItemsPerPage;
          if (0 == itemsLeft)
          {
            return this.ItemsPerPage; // exactly itemsPerPage left
          }
          else
          {
            // return the remaining items
            return itemsLeft;
          }
        }
      }
    }

    public int CurrentPage
    {
      get { return this._currentPage; }
      set
      {
        this._currentPage = value;
        this.OnPropertyChanged(new PropertyChangedEventArgs("CurrentPage"));
      }
    }

    public int ItemsPerPage { get; set; }

    public int PageCount
    {
      get
      {
        return (FilteredCount + this.ItemsPerPage - 1)
               / this.ItemsPerPage;
      }
    }

    private int EndIndex
    {
      get
      {
        var end = this._currentPage * this.ItemsPerPage - 1;
        return (end > FilteredCount) ? FilteredCount : end;
      }
    }

    private int StartIndex
    {
      get { return (this._currentPage - 1) * this.ItemsPerPage; }
    }

    public override object GetItemAt(int index)
    {
      var offset = index % (this.ItemsPerPage);
      return this.FilteredCollection.ElementAt(this.StartIndex + offset);
    }

    public void MoveToNextPage()
    {
      if (this._currentPage < this.PageCount)
      {
        this.CurrentPage += 1;
      }

      this.Refresh();
    }

    public void MoveToPreviousPage()
    {
      if (this._currentPage > 1)
      {
        this.CurrentPage -= 1;
      }

      this.Refresh();
    }
  }
}