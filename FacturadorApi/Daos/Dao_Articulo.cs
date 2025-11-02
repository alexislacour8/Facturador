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
        public Articulo RegisterArticuloAsync(string idar,string nombre, decimal precio, decimal cantidad)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(nombre) || precio < 0 || cantidad < 0)
                    return null;
                var codArt = this._context.Articulos.Where(ar =>ar.ART_ID == idar).FirstOrDefault();
                if (codArt != null) 
                {
                    return null;
                }
                

                
                    var articulo = new Articulo
                    {
                        Nombre = nombre,
                        Precio = precio,
                        Stock = cantidad,
                        ART_ID = idar
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
        

    }
}
