using System;
using System.Collections.Generic;

namespace FacturadorModels.Models;

public partial class Cliente
{
    public int Cli_ID { get; set; }

    public string RazonSocial { get; set; } = null!;

    public string Cuit { get; set; } = null!;

    public string? Direccion { get; set; }

    public bool Deshabilitado { get; set; }

    public virtual ICollection<FacturaCabecera> FacturaCabeceras { get; set; } = new List<FacturaCabecera>();
}
