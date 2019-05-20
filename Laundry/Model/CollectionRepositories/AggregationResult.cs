﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;

namespace Model.CollectionRepositories
{
  public class AggregationResult
  {
    [BsonId]
    public DateTime DateTime { get; set; }
    public double Price { get; set; }
    public long Count { get; set; }
    public double UnCountableCount { get; set; }
  }

  public class SubsidiaryAggregationResult
  {
    [BsonId] public int SubsidiaryId;
    public string Signature { get; set; }
    public double Price { get; set; }
    public long Count { get; set; }
    public double UnCountableCount { get; set; }
  }
}
