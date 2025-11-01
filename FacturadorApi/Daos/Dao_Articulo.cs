using FacturadorApi.Data;
using FacturadorApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace FacturadorApi.Daos
{
    public class Dao_Articulo
    {
        private readonly FacturadorDbContext _context;
        public Dao_Articulo(FacturadorDbContext context)
        {
            this._context = context;
        }
        public List<Articulo> GetAllArticulos()
        {
            var dataList = this._context.Articulos.Where(cod => cod.Deshabilitado == false).ToList();

            return dataList;
        }
        public Articulo GetAllbyid(string id)
        {
            var dataList = this._context.Articulos.Where(a => a.ART_ID == id).FirstOrDefault();

            return dataList;
        }
        public Articulo RegisterArticuloAsync(string nombre, decimal precio, decimal cantidad)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(nombre) || precio < 0 || cantidad < 0)
                    return null;

                // Generar código único
                string codigo;
                do
                {
                    codigo = GenerateCodigoBarra();
                } while (_context.Articulos.Any(a => a.ART_ID == codigo));

                // Crear el artículo
                var articulo = new Articulo
                {
                    Nombre = nombre,
                    Precio = precio,
                    Stock = cantidad,
                    ART_ID = codigo
                };

                this._context.Articulos.Add(articulo);
                this._context.SaveChanges();
                return articulo;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al registrar articulo: {ex.Message}");
                return null;
            }
        }

        public Articulo UpdateArticulo(Articulo articulo, string nombre, decimal precio, decimal cantidad)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(nombre) || precio < 0 || cantidad < 0)
                {
                    return null;

                }

                articulo.Nombre = nombre;
                articulo.Precio = precio;
                articulo.Stock = cantidad;

                this._context.SaveChanges();
                return articulo;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al registrar articulo: {ex.Message}");
                return null;
            }
        }
        public Articulo DeleteArticulo(Articulo articulo)
        {
            try
            {
                articulo.Deshabilitado = true;

                this._context.SaveChanges();
                return articulo;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al registrar articulo: {ex.Message}");
                return null;
            }
        }
        // Método para generar un código de barras aleatorio (puedes personalizar el formato)
        private string GenerateCodigoBarra()
        {
            var random = new Random();
            return random.Next(100000, 999999).ToString(); // Código de 6 dígitos
        }

    }
}
