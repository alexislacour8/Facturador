using System;
using System.Collections.Generic;

namespace FacturadorApi.Models;

public partial class vw_Factura_Cliente
{
    public int FC_ID { get; set; }

    public DateOnly FechaAlta { get; set; }

    public string RazonSocial { get; set; } = null!;

    public string CUIT { get; set; } = null!;
}
