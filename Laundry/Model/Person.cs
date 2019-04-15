using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Laundry.Model.DatabaseClients;
using MongoDB.Bson.Serialization.Attributes;

namespace Laundry.Model
{
  public class Person : IRepositoryElement, IDataErrorInfo
  {
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

    public string this[string columnName]
    {
      get
      {
        string error = string.Empty;
        switch (columnName)
        {
          case nameof(this.Name):
            error += string.IsNullOrEmpty(Name) ? "Имя не правильно" : string.Empty;
            break;
        }

        return error;
      }
    }

    public string Error { get; }
  }
}