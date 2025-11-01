using FacturadorApi.Data;
using FacturadorApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace FacturadorApi.Daos
{
    public class Dao_Clientes
    {
        private readonly FacturadorDbContext _context;
        public Dao_Clientes(FacturadorDbContext context)
        {
            this._context = context;
        }
        public Cliente GetById(int id)
        {
            var client = this._context.Clientes.Where(c => c.Cli_ID == id && c.Deshabilitado == false).FirstOrDefault();
            return client;
        }

        public List<Cliente> GetAll()
        {
            List<Cliente> cliente = new List<Cliente>();
            var dataList = this._context.Clientes.Where(c => c.Deshabilitado == false).ToList();

            return dataList;
        }
        public Cliente? RegsiterClientes(string RazonSocial, string CUIT, string direccion, bool Deshabilitado)
        {
            try
            {
                var validacuit = this._context.Clientes.Where(cl => cl.CUIT == CUIT).FirstOrDefault();
                if (validacuit != null)
                {

                    return null;
                }

                Cliente cliente = new Cliente();
                cliente.RazonSocial = RazonSocial;
                cliente.CUIT = CUIT;
                cliente.Direccion = direccion;
                cliente.Deshabilitado = false;

                this._context.Clientes.Add(cliente);
                this._context.SaveChanges();
                return cliente;
            }
            catch (Exception Error)
            {
                return null;
            }
        }
        public Cliente? Updatecliente(Cliente cliente, string RazonSocial, string CUIT, string direccion, bool Deshabilitado)
        {
            try
            {

                cliente.RazonSocial = RazonSocial;
                cliente.CUIT = CUIT;
                cliente.Direccion = direccion;

                this._context.SaveChanges();
                return cliente;
            }
            catch (Exception Error)
            {
                return null;
            }
        }
        public bool DesactiveClient(int id)
        {
            try
            {
                var client = this.GetById(id);
                if (client == null)
                {
                    return false;
                }

                client.Deshabilitado = true;

                this._context.SaveChanges();
                return true;
            }
            catch (Exception error)
            {
                return false;
            }
        }

    }
}
