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
using Laundry.Model;

namespace Laundry.Utils
{
  /// <summary>
  /// Interaction logic for ActivityControl.xaml
  /// </summary>
  public class ActivityControl : UserControl
  {
    //Bool props for disabling or enabling neccessary controls that depends on profession status

    public bool IsCourier => App.Model.CurrentUser.Profession == EmployeeProfession.Courier;
    public bool IsDirector => App.Model.CurrentUser.Profession == EmployeeProfession.Director;
    public bool IsAdvisor => App.Model.CurrentUser.Profession == EmployeeProfession.Advisor;
    public bool IsWasher => App.Model.CurrentUser.Profession == EmployeeProfession.Washer;


    public ActivityControl(UserControl context = null)
    {
    }
  }
}
