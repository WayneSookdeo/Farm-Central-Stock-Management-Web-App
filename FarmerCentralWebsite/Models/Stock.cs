using System;
using System.Collections.Generic;

namespace FarmerCentralWebsite.Models;

public partial class Stock
{
    public int StockId { get; set; }

    public int ProductId { get; set; }

    public string FarmerEmail { get; set; } = null!;

    public virtual Farmer FarmerEmailNavigation { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
