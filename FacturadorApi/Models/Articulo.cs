using System;
using System.Collections.Generic;

namespace FacturadorApi.Models;

public partial class Articulo
{
    public string ART_ID { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public decimal Precio { get; set; }

    public decimal? Stock { get; set; }

    public virtual ICollection<Factura_Detalle> Factura_Detalles { get; set; } = new List<Factura_Detalle>();
    public bool Deshabilitado { get; set; } = false;

}
