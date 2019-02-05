using System;
using System.Collections.ObjectModel;

namespace Laundry.Model
{
  public class Client
  {
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Patronymic { get; set; }
    public DateTime DateBirth { get; set; }
    public ObservableCollection<Order> Orders { get; set; }
    public string PhoneNumber { get; set; }
    public int House { get; set; }
    public string Street { get; set; }
    public string City { get; set; }
    public int ZipCode { get; set; }
    public bool IsPremiumClient { get; set; }

    public Client(string name, string surname, string patronymic)
    {
      Name = name;
      Surname = surname;
      Patronymic = patronymic;
      DateBirth = new DateTime();
      Orders = new ObservableCollection<Order>();
    }
  }
}