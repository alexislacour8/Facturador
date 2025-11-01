using System;
using System.Collections.Generic;

namespace FacturadoBlazor.Models;

public partial class Cliente
{
    public int Cli_ID { get; set; }

    public string RazonSocial { get; set; } = null!;

    public string CUIT { get; set; } = null!;

    public string? Direccion { get; set; }

    public bool Deshabilitado { get; set; }

}
