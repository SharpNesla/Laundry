using System;
using System.Collections.ObjectModel;

namespace Laundry.Model
{
  public enum EmployeeProfession
  {
    Courier,
    Director,
    Washer,
    Advisor,
    Driver
  }

  public class Employee
  {
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Patronymic { get; set; }
    public DateTime DateBirth { get; set; }
    public EmployeeProfession Profession { get; set; }

    public int House { get; set; }
    public string Street { get; set; }
    public string City { get; set; }
    public int ZipCode { get; set; }

    public Employee(string name, string surname, string patronymic)
    {
      Name = name;
      Surname = surname;
      Patronymic = patronymic;
      DateBirth = new DateTime();
    }
  }
}