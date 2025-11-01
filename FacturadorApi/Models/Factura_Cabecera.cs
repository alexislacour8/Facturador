using System;
using System.Collections.Generic;

namespace FacturadorApi.Models;

public partial class Factura_Cabecera
{
    public int FC_ID { get; set; }

    public DateOnly FechaAlta { get; set; }

    public int Cli_ID { get; set; }

    public string? Estado { get; set; }

    public virtual Cliente Cli { get; set; } = null!;

    public virtual ICollection<Factura_Detalle> Factura_Detalles { get; set; } = new List<Factura_Detalle>();
}
