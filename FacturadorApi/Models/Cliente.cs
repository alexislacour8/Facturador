using System;
using System.Collections.Generic;

namespace FacturadorApi.Models;

public partial class Cliente
{
    public int Cli_ID { get; set; }

    public string RazonSocial { get; set; } = null!;

    public string CUIT { get; set; } = null!;

    public string? Direccion { get; set; }

    public bool Deshabilitado { get; set; }

    public virtual ICollection<Factura_Cabecera> Factura_Cabeceras { get; set; } = new List<Factura_Cabecera>();
}
