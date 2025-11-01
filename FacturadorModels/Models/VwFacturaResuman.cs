using System;
using System.Collections.Generic;

namespace FacturadorModels.Models;

public partial class VwFacturaResuman
{
    public int FC_ID { get; set; }

    public string? Estado { get; set; }

    public decimal? TotalFactura { get; set; }
}
