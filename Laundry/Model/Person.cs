using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Laundry.Model.DatabaseClients;
using MongoDB.Bson.Serialization.Attributes;

namespace Laundry.Model
{
  public enum Gender
  {
    Male,
    Female
  }

  public class Person : IRepositoryElement, IDataErrorInfo, INotifyDataErrorInfo
  {
    [BsonId]
    public long Id { get; set; }

    [BsonIgnoreIfNull]
    public string Name { get; set; }

    [BsonIgnoreIfNull]
    public string Surname { get; set; }

    [BsonIgnoreIfNull]
    public string Patronymic { get; set; }

    [BsonIgnoreIfNull]
    public string PhoneNumber { get; set; }

    [BsonIgnoreIfDefault]
    public DateTime DateBirth { get; set; }
    
    public Gender Gender { get; set; }

    [BsonIgnoreIfNull]
    public string House { get; set; }

    [BsonIgnoreIfNull]
    public string Street { get; set; }

    [BsonIgnoreIfNull]
    public string City { get; set; }

    [BsonIgnoreIfNull]
    public string Flat { get; set; }

    [BsonIgnoreIfNull]
    public string ZipCode { get; set; }

    [BsonIgnoreIfDefault]
    public DateTime DeletionDate { get; set; }

    [BsonIgnore]
    public virtual string Signature
    {
      get { return $"{this.Id} {this.Name} {this.Surname}"; }
    }
    [BsonIgnore]
    public bool IsSelected { get; set; }

    public string this[string columnName]
    {
      get
      {
        string error = string.Empty;
        switch (columnName)
        {
          case nameof(this.Name):
            if (this.Name == null)
              error = "Имя не может быть пустым полем";
            else if(!Regex.IsMatch(this.Name, @"^[а-яА-Я-а-яА-Я ]*([а-я])$")) { 
              error = "Имя содержит не правильные символы";
            }
            break;
          case nameof(this.Surname):
            error = string.IsNullOrWhiteSpace(Surname) ? "Фамилия не может быть пустым полем" : string.Empty;
            break;
        }

        return error;
      }
    }

    

    public string Error { get; }
    public IEnumerable GetErrors(string propertyName)
    {
      return this[propertyName];
    }

    public bool HasErrors { get; } = false;

    public void TriggerValidation()
    {
      this.ErrorsChanged?.Invoke(null, null);
    }
    public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
  }
}