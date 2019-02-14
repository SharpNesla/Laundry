using System;
using System.Collections.Generic;
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
using Laundry.Model;
using MaterialDesignThemes.Wpf;

namespace Laundry.Utils
{
  /// <summary>
  /// Interaction logic for ActivityControl.xaml
  /// </summary>
  public class ActivityControl : UserControl
  {
    //Bool props for disabling or enabling neccessary controls that depends on profession status

    public Visibility Courier => App.Model.CurrentUser.Profession == EmployeeProfession.Courier ? Visibility.Visible: Visibility.Hidden;
    public Visibility Director => App.Model.CurrentUser.Profession == EmployeeProfession.Director ? Visibility.Visible : Visibility.Hidden;
    public Visibility Washer => App.Model.CurrentUser.Profession == EmployeeProfession.Washer ? Visibility.Visible : Visibility.Hidden;
    public Visibility Advisor => App.Model.CurrentUser.Profession == EmployeeProfession.Advisor ? Visibility.Visible : Visibility.Hidden;

    protected UserControl Context { get;}

    public ActivityControl(UserControl context)
    {
      this.Context = context;
    }

    protected void ReturnContext()
    {
      App.CurrentWindow.ChangeView(Context);
    }

    public static void BindMainDrawerButton(ToggleButton button)
    {
      App.CurrentWindow.DrawerHost.GetBindingExpression(DrawerHost.IsLeftDrawerOpenProperty)
        ?.UpdateTarget();
    }
  }
}
