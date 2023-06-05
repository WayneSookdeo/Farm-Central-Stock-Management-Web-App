using System;
using System.Collections.Generic;

namespace FarmerCentralWebsite.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string? ProductName { get; set; }

    public string? ProductType { get; set; }

    public double? StockPrice { get; set; }

    public DateTime? DateSupplied { get; set; }

    public virtual ICollection<Stock> Stocks { get; set; } = new List<Stock>();
}
