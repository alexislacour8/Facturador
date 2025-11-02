using FacturadorApi.Daos;
using FacturadorApi.Data;
using FacturadorApi.FomrsInputs;
using FacturadorApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static FacturadorApi.Daos.Dao_Factura;
using static FacturadorApi.FomrsInputs.Forms;
using static FacturadorApi.FomrsInputs.FormsFactura;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FacturadorApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FacturaController : ControllerBase
    {
        private readonly FacturadorDbContext _context;
        private readonly Dao_Factura dao_Factura;
        public FacturaController(FacturadorDbContext context)
        {
            _context = context;
            this.dao_Factura = new Dao_Factura(context);
        }
        // GET: api/<FacturaController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<FacturaController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [HttpPost("CreateFactura")]
        public async Task<IActionResult> CreateFactura(FomrsInputs.FormsFactura.Factura factura)
        {
            if (factura == null || factura.Detalles.Count == 0)
                return BadRequest("Debe enviar al menos un artículo.");

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
               
                var cabecera = dao_Factura.CreateFacturaCabezera(factura.cuit, factura.Estado);
                if (cabecera == null)
                    throw new Exception("No se pudo crear la cabecera de la factura.");

                foreach (var item in factura.Detalles)
                {
                    var detalle = dao_Factura.CreateFacturaDetalle(
                        cabecera.FC_ID,
                        item.ART_ID,
                        item.Cant,
                        item.Precio
                    );

                    if (detalle == null)
                        throw new Exception($"Error al crear detalle del artículo {item.ART_ID}");
                }

                await transaction.CommitAsync();
                return Ok(new { Mensaje = "Factura creada correctamente", cabecera.FC_ID });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, $"Error al crear la factura: {ex.Message}");
            }
        }
        [HttpGet("GetAllFacturas")]
        public IActionResult GetAllfacturas()
        {


            var facturas = this.dao_Factura.GenerarFacturaParaMostrar();

            return Ok(new { message = "Datos recibidos", data = facturas });
        }
       

       
        [HttpPut("PutUpdateFacturas/{id}")]
        public IActionResult UpdateCabezera(FomrsInputs.Forms.FacturaVista factura,int id)
        {
            try
            {
                var factu = this.dao_Factura.GetByIdFactura(id);
                if (factu == null)
                {
                    return BadRequest("No se encontro ninguna factura");
                }
                var facturacabezera = this.dao_Factura.UpdateFacturaCabezera(factu, factura.FechaAlta,factura.Estado);
                return Ok(new { Mensaje = "Factura editada correctamente", facturacabezera });

            }
            catch (Exception ex) 
            {
                return StatusCode(500, $"Error al crear la factura: {ex.Message}");
            }
        }
        [HttpPut("UpdateFacturaDetalle")]
        public IActionResult UpdateFacturaDetalle(FacturaVista facturaDetalle)
        {
            try
            {
               
                var factura = dao_Factura.GetByIdFactura(facturaDetalle.FC_ID);
                if (factura == null)
                    return BadRequest("No se encontró la factura");

              
                var detalleNuevo = facturaDetalle.Detalles.FirstOrDefault();
                if (detalleNuevo == null)
                    return BadRequest("No se envió ningún detalle");

               
                var detalleViejo = this.dao_Factura.GetByIdFacturaDetalle(detalleNuevo.FC_DTL_ID);
                if (detalleViejo == null)
                    return BadRequest("No se encontró el detalle en la base");

               
                detalleViejo.Cant = detalleNuevo.Cant;
                detalleViejo.Precio = detalleNuevo.Precio;
                detalleViejo.Monto = detalleNuevo.Cant * detalleNuevo.Precio;

              
                this._context.SaveChanges();

                return Ok(new { Mensaje = "Detalle actualizado correctamente", detalleViejo });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al editar el detalle: {ex.Message}");
            }
        }

        [HttpDelete("RemoveFacturaDetalle/{idDetalle}")]
        public IActionResult RemoveFacturaDetalle(int idDetalle)
        {
            try
            {
                // Buscamos el detalle en la base
                var detalle = dao_Factura.GetByIdFacturaDetalle(idDetalle);
                if (detalle == null)
                    return NotFound("No se encontró el detalle a eliminar.");

                _context.Remove(detalle);
                _context.SaveChanges();

                return Ok(new { Mensaje = "Detalle eliminado correctamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al eliminar el detalle: {ex.Message}");
            }
        }


        [HttpPatch("PatchCancelinvoice/{id}")]
        public IActionResult PatchCancelinvoice(int id)
        {
            try
            {
                var factu = this.dao_Factura.GetByIdFactura(id);
                if (factu == null)
                {
                    return BadRequest("No se encontro ninguna factura");
                }
                var facturacabezera = this.dao_Factura.CancelInvoice(factu);
                return Ok(new { Mensaje = "Factura editada correctamente", facturacabezera });

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al crear la factura: {ex.Message}");
            }
        }
        [HttpGet("Vista1")]
        public async Task<ActionResult<List<vw_Factura_Resuman>>> GetVista1()
        {
            var facturas = await this._context.vw_Factura_Resumen.ToListAsync();
            return Ok(new { message = "Datos recibidos", data = facturas });
        }
        [HttpGet("Vista2")]
        public async Task<ActionResult<List<vw_Factura_Cliente>>> GetVista2()
        {
            var facturas = await this._context.vw_Factura_Clientes.ToListAsync();
            return Ok(new { message = "Datos recibidos", data = facturas });
        }
        [HttpGet("PorClienteMasVendido")]
        public async Task<IActionResult> GetPorClienteMasVendido(DateTime fechaDesde, DateTime fechaHasta, int idCliente)
        {
            var (facturas, producto) = await this.dao_Factura.ObtenerFacturasYProductoMasVendido(fechaDesde, fechaHasta, idCliente);

            return Ok(new
            {
                Facturas = facturas,
                ProductoMasVendido = producto
            });
        }
        [HttpGet("FactuClienteConDetalle")]
        public async Task<IActionResult> GetFacturasConDetalle(DateTime fechaDesde, DateTime fechaHasta, int idCliente)
        {
            var facturas = await this.dao_Factura.ObtenerFacturasConDetalle(fechaDesde, fechaHasta, idCliente);
            return Ok(facturas);
        }
    }
}
