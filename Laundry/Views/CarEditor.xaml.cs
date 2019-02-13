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
using Laundry.Utils;

namespace Laundry.Views
{
  /// <summary>
  /// Interaction logic for CarEditor.xaml
  /// </summary>
  public partial class CarEditor : ActivityControl
  {
    public CarEditor(UserControl context) : base(context)
    {
      InitializeComponent();
    }

    private void OnDisableButtonClick(object sender, RoutedEventArgs e)
    {
      App.CurrentWindow.ChangeView(this.Context);
    }
  }
}