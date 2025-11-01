using FacturadorApi.Daos;
using FacturadorApi.Data;
using FacturadorApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FacturadorAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private readonly FacturadorDbContext _context;
        private readonly Dao_Clientes dao_Clientes;
        public ClientesController(FacturadorDbContext context)
        {
            _context = context;
            this.dao_Clientes = new Dao_Clientes(context);
        }

        // GET: api/clientes
        [HttpGet]
        public IActionResult GetAllclientes()
        {
            var ListClientes = this.dao_Clientes.GetAll();
            if (ListClientes.Count == 0)
            {
                return NotFound(new { message = "No se encontraron Clientes" });
            }
            return Ok(ListClientes);
        }

        // GET: api/clientes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Cliente>> GetCliente(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null) return NotFound();
            return cliente;
        }

        // POST: api/clientes
        [HttpPost]
        public async Task<ActionResult<Cliente>> AddClientes(Cliente cliente)
        {
            try
            {
                var newClient = this.dao_Clientes.RegsiterClientes(cliente.RazonSocial, cliente.CUIT, cliente.Direccion, cliente.Deshabilitado);
                if (newClient == null)
                {
                    return StatusCode(500, new { message = "Error al guardar el cliente" });
                }

                return Ok(new { message = "newClient actualizado con éxito", client = newClient });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error procesando la solicitud", error = ex.Message });
            }
        }

        // PUT: api/clientes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateClientes(int id, Cliente cliente)
        {
            try
            {
                if (String.IsNullOrEmpty(cliente.RazonSocial) || String.IsNullOrEmpty(cliente.CUIT) || String.IsNullOrEmpty(cliente.Direccion))
                {
                    return BadRequest(new { message = "Hay campos de informacion vacios" });
                }
                var clienteupdate = this.dao_Clientes.GetById(id);
                if (clienteupdate == null)
                {
                    return BadRequest(new { message = "el usuario no existe" });
                }
                var result = this.dao_Clientes.Updatecliente(clienteupdate,
                  cliente.RazonSocial, cliente.CUIT, cliente.Direccion, cliente.Deshabilitado);
                if (result == null)
                {
                    return BadRequest(new { message = "Hubo un error al editar el cliente. Intente nuevamente." });
                }



                return Ok(new { message = "cliente editado correctamente." + result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Hubo un error al Editar el cliente. " + ex.Message });
            }
        }

        // DELETE: api/clientes/5
        [HttpPatch("{id}")]
        public async Task<IActionResult> DeleteCliente(int id)
        {
            try
            {
                var result = this.dao_Clientes.DesactiveClient(id);

                if (result == false)
                {
                    return BadRequest(new { message = "Hubo un error al Eliminar el cliente. Intente nuevamente." });
                }


                return Ok(new { message = "Cliente Eliminada correctamente." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Hubo un error al Eliminar la sucursal. " + ex.Message });
            }
        }
    }
}

