using FacturadorApi.Data;
using FacturadorApi.FomrsInputs;
using FacturadorApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using System.ComponentModel;
using System.Data;
using static FacturadorApi.FomrsInputs.Forms;
namespace FacturadorApi.Daos
{
    public class Dao_Factura
    {
        private readonly FacturadorDbContext _context;
        public Dao_Factura(FacturadorDbContext context) 
        {
            this._context = context;
        }
        public List<FomrsInputs.Forms.FacturaVista> GenerarFacturaParaMostrar()
        {
            // Traemos todas las facturas con sus detalles y datos de cliente/artículo
            var query = from f in _context.Factura_Cabeceras
                        join fd in _context.Factura_Detalles on f.FC_ID equals fd.Fact_ID
                        join c in _context.Clientes on f.Cli_ID equals c.Cli_ID
                        join art in _context.Articulos on fd.ART_ID equals art.ART_ID
                        where f.Estado !="Anulada"
                        orderby f.FC_ID, fd.FC_DTL_ID
                        select new
                        {
                            f.FC_ID,
                            f.Cli_ID,
                            f.Estado,
                            f.FechaAlta,
                            DetalleID = fd.FC_DTL_ID,
                            fd.ART_ID,
                            fd.Cant,
                            fd.Precio,
                            Monto = fd.Cant * fd.Precio
                        };

            // Agrupamos por cabecera
            var facturasAgrupadas = query
                .AsEnumerable() // Para poder usar GroupBy en memoria
                .GroupBy(x => new { x.FC_ID, x.Cli_ID, x.Estado, x.FechaAlta })
                .Select(g => new FomrsInputs.Forms.FacturaVista
                {
                    FC_ID = g.Key.FC_ID,
                    Cli_ID = g.Key.Cli_ID,
                    Estado = g.Key.Estado,
                    FechaAlta = g.Key.FechaAlta.ToDateTime(new TimeOnly(0, 0)),
                    Detalles = g.Select(d => new FomrsInputs.Forms.FacturaDetalleVista
                    {
                        FC_DTL_ID = d.DetalleID,
                        ART_ID = d.ART_ID,
                        Cant = d.Cant,
                        Precio = d.Precio,
                        Monto = d.Monto
                    }).ToList()
                })
                .ToList();

            return facturasAgrupadas;
        }



        public Factura_Cabecera CreateFacturaCabezera(string cuit, string estado)
        {
            try
            {
                var cod = this._context.Clientes.Where(c => c.CUIT == cuit).FirstOrDefault();
                var fecha = DateOnly.FromDateTime(DateTime.Now);
                if (cod == null)
                {
                    return null;
                }
                var factura = new Factura_Cabecera
                {
                    Cli_ID = cod.Cli_ID,
                    FechaAlta = fecha,
                    Estado = estado
                };
                _context.Factura_Cabeceras.Add(factura);
                this._context.SaveChanges();
                return factura;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public Factura_Cabecera UpdateFacturaCabezera(Factura_Cabecera factura , DateTime fecha)
        {
            try
            {
                factura.FechaAlta = DateOnly.FromDateTime(fecha);

                this._context.SaveChanges();
                return factura;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public Factura_Cabecera CancelInvoice(Factura_Cabecera factura)
        {
            try
            {
                factura.Estado = "Anulada";

                this._context.SaveChanges();
                return factura;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public Factura_Detalle CreateFacturaDetalle(int fact_ID, string art_ID, decimal cant, decimal precio)
        {
            try
            {
                var fecha = DateOnly.FromDateTime(DateTime.Now);
                var detalle = new Factura_Detalle
                {
                    Fact_ID = fact_ID,
                    FechaAlta = fecha,
                    ART_ID = art_ID,
                    Cant = cant,
                    Precio = precio,
                    Monto = cant * precio
                };

                _context.Factura_Detalles.Add(detalle);
                _context.SaveChanges();

                return detalle;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public Factura_Cabecera GetByIdFactura( int id) 
        {
            var facturas = this._context.Factura_Cabeceras.FirstOrDefault(f=> f.FC_ID == id);
            return facturas;
        }
        public async Task<(List<FacturaCabecera> facturas, ProductoMasVendido producto)>
       ObtenerFacturasYProductoMasVendido(DateTime fechaDesde, DateTime fechaHasta, int idCliente)
        {
            using var connection = _context.Database.GetDbConnection();
            await connection.OpenAsync();

            using var command = connection.CreateCommand();
            command.CommandText = "FacturasPorClienteProductoMasVendido";
            command.CommandType = CommandType.StoredProcedure;

            // Agregamos los parámetros
            command.Parameters.Add(new SqlParameter("@FechaDesde", fechaDesde));
            command.Parameters.Add(new SqlParameter("@FechaHasta", fechaHasta));
            command.Parameters.Add(new SqlParameter("@IDCliente", idCliente));

            var facturas = new List<FacturaCabecera>();
            ProductoMasVendido producto = null;

            using var reader = await command.ExecuteReaderAsync();

            // 1️⃣ Primer SELECT → facturas
            while (await reader.ReadAsync())
            {
                facturas.Add(new FacturaCabecera
                {
                    FC_ID = reader.GetInt32(reader.GetOrdinal("FC_ID")),
                    Cli_ID = reader.GetInt32(reader.GetOrdinal("Cli_ID")),
                    FechaAlta = reader.GetDateTime(reader.GetOrdinal("FechaAlta")),
                    Estado = reader.GetString(reader.GetOrdinal("Estado"))
                });
            }

            // 2️⃣ Segundo SELECT → producto más vendido
            if (await reader.NextResultAsync() && await reader.ReadAsync())
            {
                producto = new ProductoMasVendido
                {
                    ART_ID = reader.GetString(reader.GetOrdinal("ART_ID")),
                    Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                    TotalCantidad = reader.GetDecimal(reader.GetOrdinal("TotalCantidad"))
                };
            }

            return (facturas, producto);
        }


    }
}
