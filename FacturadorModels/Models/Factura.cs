using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FacturadorModels.Models
{

    using System.Text.Json.Serialization;

    using System.Text.Json.Serialization;

    public class FacturaVista
    {
        public int Cli_ID { get; set; }
        public int FC_ID { get; set; }
        public string Estado { get; set; }
        public DateTime FechaAlta { get; set; }
        public List<FacturaDetalleVista> Detalles { get; set; } = new();
    }

    public class CreateFactura
    {

        public string Cuit { get; set; }

        public string Estado { get; set; } = "Facturado";

      
        public List<FacturaDetalleVista> Detalles { get; set; } = new();
    }
    public class FacturaDetalleVista
    {
        [JsonPropertyName("fC_DTL_ID")]
        public int FC_DTL_ID { get; set; }

        [JsonPropertyName("arT_ID")]
        public string ART_ID { get; set; }

        [JsonPropertyName("cant")]
        public decimal Cant { get; set; }

        [JsonPropertyName("precio")]
        public decimal Precio { get; set; }

        [JsonPropertyName("monto")]
        public decimal Monto { get; set; }
    }
    public class ApiResponse<T>
    {
        public string Message { get; set; } = string.Empty;
        public T Data { get; set; } = default!;
    }


}
