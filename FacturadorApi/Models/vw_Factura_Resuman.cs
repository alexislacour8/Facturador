using System;
using System.Collections.Generic;

namespace FacturadorApi.Models;

public partial class vw_Factura_Resuman
{
    public int FC_ID { get; set; }

    public string? Estado { get; set; }

    public decimal? TotalFactura { get; set; }
}
