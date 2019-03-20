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
  public partial class Paginator: UserControl
  {
    
    public string FieldName
    {
      get { return (string)GetValue(FieldNameProperty); }
      set { SetValue(FieldNameProperty, value); }
    }



    public ICollectionView Orders
    {
      get { return (ICollectionView)GetValue(OrdersProperty); }
      set
      {
        this.DataContext = this;
        SetValue(OrdersProperty, value);
      }
    }

    // Using a DependencyProperty as the backing store for Orders.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty OrdersProperty =
      DependencyProperty.Register("Orders", typeof(ICollectionView), typeof(Paginator), new PropertyMetadata(PropChanged));

    public static void PropChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
    {
      var value = e.NewValue; //confirm this isn't null
    }

    // Using a DependencyProperty as the backing store for FieldName.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty FieldNameProperty =
        DependencyProperty.Register("FieldName", typeof(string), typeof(Paginator));




    public bool IsMovePreviousEnabled { get; set; } = true;
    public bool IsMoveNextEnabled { get; set; } = true;


    public Paginator()
    {
      InitializeComponent();
      this.DataContext = this;
    }
    
    private void OnMovePreviousButtonClick(object sender, RoutedEventArgs e)
    {
    }

    private void OnMoveNextButtonClick(object sender, RoutedEventArgs e)
    {
    }
  }
}
