using System;
using System.Collections.Generic;

namespace FarmerCentralWebsite.Models;

public partial class Farmer
{
    public string FarmerEmail { get; set; } = null!;

    public string FarmerName { get; set; } = null!;

    public string? Password { get; set; }

    public virtual ICollection<Stock> Stocks { get; set; } = new List<Stock>();
}
