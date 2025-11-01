using System;
using System.Collections.Generic;

namespace FacturadorModels.Models;

public partial class VwFacturaCliente
{
    public int FC_ID { get; set; }

    public DateOnly FechaAlta { get; set; }

    public string RazonSocial { get; set; } = null!;

    public string Cuit { get; set; } = null!;
}
