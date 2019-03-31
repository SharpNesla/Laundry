using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laundry.Model
{
  public class ClothInstance
  {
    public ClothKind Kind { get; set; }
    public int WearPercentage { get; set; }
    public int Amount { get; set; }
    public int Id { get; set; }
  }
}