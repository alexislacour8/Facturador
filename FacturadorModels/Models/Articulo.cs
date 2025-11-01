using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FacturadorModels.Models;

public partial class Articulo
{
    [JsonPropertyName("ART_ID")]
    public string ArtId { get; set; } = null!;

    public string Nombre { get; set; } = null!;
    public decimal Precio { get; set; }
    public decimal? Stock { get; set; }

}

