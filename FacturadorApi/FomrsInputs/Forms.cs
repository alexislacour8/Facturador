namespace FacturadorApi.FomrsInputs
{
    public class Forms
    {
        public class FacturaVista
        {
            public int Cli_ID { get; set; }
            public int FC_ID { get; set; }
            public string Estado { get; set; }
            public DateTime FechaAlta { get; set; }

            public List<FacturaDetalleVista> Detalles { get; set; } = new();
        }

        public class FacturaDetalleVista
        {
            public int FC_DTL_ID { get; set; }
            public string ART_ID { get; set; } = string.Empty;
            public decimal Cant { get; set; }
            public decimal Precio { get; set; }
            public decimal Monto { get; set; }
        }
    }
}
