using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Model;

namespace Laundry.Views
{
  public class Visibilities
  {
    private readonly IModel _model;

    public Visibility Courier => _model.CurrentUser.Profession == EmployeeProfession.Courier ? Visibility.Visible : Visibility.Collapsed;
    public Visibility Advisor => _model.CurrentUser.Profession == EmployeeProfession.Advisor ? Visibility.Visible : Visibility.Collapsed;
    public Visibility Driver => _model.CurrentUser.Profession == EmployeeProfession.Driver ? Visibility.Visible : Visibility.Collapsed;
    public Visibility Washer => _model.CurrentUser.Profession == EmployeeProfession.Washer ? Visibility.Visible : Visibility.Collapsed;
    public Visibility Director => _model.CurrentUser.Profession == EmployeeProfession.Director ? Visibility.Visible : Visibility.Collapsed;
    

    public Visibility DirectorAdvisor =>
      _model.CurrentUser.Profession == EmployeeProfession.Director ||
      _model.CurrentUser.Profession == EmployeeProfession.Advisor
        ? Visibility.Visible
        : Visibility.Collapsed;

    public Visibilities(IModel model)
    {
      _model = model;
    }
  }
}
