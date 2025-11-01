using System;
using System.Collections.Generic;

namespace FacturadorApi.Models;

public partial class Factura_Detalle
{
    public int Fact_ID { get; set; }

    public int FC_DTL_ID { get; set; }

    public DateOnly FechaAlta { get; set; }

    public string ART_ID { get; set; } = null!;

    public decimal Cant { get; set; }

    public decimal Precio { get; set; }

    public decimal Monto { get; set; }

    public virtual Articulo ART { get; set; } = null!;

    public virtual Factura_Cabecera Fact { get; set; } = null!;
}
