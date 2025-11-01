using System;
using System.Collections.Generic;

namespace FacturadorModels.Models;

public partial class VwFacturaResuman
{
    public int FcId { get; set; }

    public string? Estado { get; set; }

    public decimal? TotalFactura { get; set; }
}
