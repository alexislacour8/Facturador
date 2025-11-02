using System;
using System.Collections.Generic;

namespace FacturadorModels.Models;

public partial class FacturaCabecera
{
    public int FC_ID { get; set; }

    public DateTime FechaAlta { get; set; }

    public int Cli_Id { get; set; }

    public string? Estado { get; set; }

    public virtual Cliente Cli { get; set; } = null!;

    public virtual ICollection<FacturaDetalle> FacturaDetalles { get; set; } = new List<FacturaDetalle>();
}
