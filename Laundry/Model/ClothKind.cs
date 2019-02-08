using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laundry.Model
{
  public class ClothKind
  {
    public string Name { get; set; }
    public MeasureKind MeasureKind { get; set; }
    public float Price { get; set; }
  }
}