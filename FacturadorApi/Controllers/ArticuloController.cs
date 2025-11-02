using FacturadorApi.Daos;
using FacturadorApi.Data;
using FacturadorApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static FacturadorApi.Controllers.ArticuloController;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FacturadorApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticuloController : ControllerBase
    {
        private readonly FacturadorDbContext _context;
        private readonly Dao_Articulo dao_Articulo;
        public ArticuloController(FacturadorDbContext context)
        {
            _context = context;
            this.dao_Articulo = new Dao_Articulo(context);
        }
        // GET: api/<ArticuloController>
        [HttpGet("GetArticulo")]
        public IActionResult GetArticulo()
        {
            try
            {

                var articuloList = this.dao_Articulo.GetAllArticulos();
                if (articuloList == null)
                {
                    return NotFound(new { message = "No se encontraron productos" });
                }


                return Ok(articuloList);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "error get product" });
            }
        }

        // GET api/<ArticuloController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

       
        [HttpPost("AddArticulo")]
        public IActionResult AddArticulo(Articulo articulo)
        {
            try
            {
                var productosRegistrados = this.dao_Articulo.RegisterArticuloAsync(
                    articulo.ART_ID,
                    articulo.Nombre,
                    articulo.Precio,
                    articulo.Stock ?? 0
                );

                if (productosRegistrados == null)
                {
                    return StatusCode(500, new { message = "Error al registrar el Articulo. este codigo ya existe" });
                }

                return Ok(new { productos = productosRegistrados, message = "Producto registrado correctamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ocurrió un error al procesar la solicitud.", error = ex.Message });
            }
        }

        public partial class Articuloupdate
        {
            public string Nombre { get; set; } = null!;
            public decimal Precio { get; set; }
            public decimal? Stock { get; set; }
        }

        // PUT api/<ArticuloController>/5
        [HttpPut("UpdateArticulo/{id}")]
        public IActionResult UpdateArticulo([FromBody] Articuloupdate articulo , string id)
        {
            try
            {
                var Articulo = this.dao_Articulo.GetAllbyid(id);
                var productosRegistrados = this.dao_Articulo.UpdateArticulo(Articulo,
                    articulo.Nombre,
                    articulo.Precio,
                    articulo.Stock ?? 0
                );

                if (productosRegistrados == null)
                {
                    return StatusCode(500, new { message = "Error al registrar el producto." });
                }

                return Ok(new { productos = productosRegistrados, message = "Producto registrado correctamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ocurrió un error al procesar la solicitud.", error = ex.Message });
            }
        }

        // DELETE api/<ArticuloController>/5
        [HttpPatch("DeleteArticulo/{id}")]
        public IActionResult DeleteArticulo(string id)
        {
            try
            {
                var Articulo = this.dao_Articulo.GetAllbyid(id);



                var deletearticulo = this.dao_Articulo.DeleteArticulo(Articulo);

                return Ok(new { productos = deletearticulo, message = "Articulo Eliminado correctamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ocurrió un error al procesar la solicitud de eliminar Articulo.", error = ex.Message });
            }
        }
    }
}
