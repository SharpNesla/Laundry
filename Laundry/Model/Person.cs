﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Model.DatabaseClients;
using MongoDB.Bson.Serialization.Attributes;

namespace Model
{
  public enum Gender
  {
    Male,
    Female,
    Undefined
  }

  public class Person : IRepositoryElement, IDataErrorInfo
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
            
            break;
          case nameof(this.Surname):
            error = string.IsNullOrWhiteSpace(Surname) ? "Фамилия не может быть пустым полем" : string.Empty;
            break;
        }

        return error;
      }
    }

    public string Error { get; }

    public override string ToString()
    {
      return $"{this.Surname} {this.Name} {this.Patronymic}";
    }
  }
}