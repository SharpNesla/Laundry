using System;


namespace Laundry.Model
{

  public enum CarCategory
  {
    A,B,C,D,E
  }

  public class Car
  {
    public string Sign { get; set; }
    public string BrandAndModel { get; set; }
    public string VIN { get; set; }

    //public CarType Type { get; set; }

    public CarCategory Category { get; set; }
    public DateTime CreationYear;
    public string BodyID { get; private set; }
    public string Color { get; set; }
  }
}