using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laundry.Model
{
  public class Order
  {
    public string Comment { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime ExecutionDate { get; set; }
    public ObservableCollection<ClothInstance> ClothInstances { get; set; }
    public float Price { get; set; }

    public Order()
    {
      this.ClothInstances = new ObservableCollection<ClothInstance>();
    }
  }
}