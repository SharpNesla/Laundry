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

    public bool Courier => _model.CurrentUser.Profession == EmployeeProfession.Courier;
    public bool Advisor => _model.CurrentUser.Profession == EmployeeProfession.Advisor;
    public bool Driver => _model.CurrentUser.Profession == EmployeeProfession.Driver;
    public bool Washer => _model.CurrentUser.Profession == EmployeeProfession.Washer;
    public bool Director => _model.CurrentUser.Profession == EmployeeProfession.Director;

    public bool DirectorAdvisor =>
      _model.CurrentUser.Profession == EmployeeProfession.Director ||
      _model.CurrentUser.Profession == EmployeeProfession.Advisor;

    public Visibilities(IModel model)
    {
      _model = model;
    }
  }
}
