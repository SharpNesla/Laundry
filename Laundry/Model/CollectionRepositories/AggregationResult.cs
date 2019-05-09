using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laundry.Model.CollectionRepositories
{
  public class AggregationResult
  {
    public DateTime DateTime { get; set; }
    public double Price { get; set; }
    public long Amount { get; set; }
  }
}
