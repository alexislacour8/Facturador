namespace FacturadorApi.FomrsInputs
{
    public class FormsFactura
    {
        public class Factura
        {
           
            public string cuit { get; set; }
            public string Estado { get; set; }
            public List<FacturaDetalleItem> Detalles { get; set; } = new List<FacturaDetalleItem>();
        }

        public class FacturaDetalleCliente
        {
            public int FC_ID { get; set; }
            public DateTime FechaAlta { get; set; }
            public int Cli_ID { get; set; }
            public string Estado { get; set; }

            public int FC_DTL_ID { get; set; }
            public string ART_ID { get; set; }
            public decimal Cant { get; set; }
            public decimal Precio { get; set; }
            public decimal Monto { get; set; }
        }

        public class FacturaDetalleItem
        {
           
            public int FC_DTL_ID { get; set; }

            public string ART_ID { get; set; } = string.Empty;

         
            public decimal Cant { get; set; }

         
            public decimal Precio { get; set; }

          
            public decimal Monto;
        }

    }
}
