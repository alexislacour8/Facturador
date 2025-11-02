using Azure;
using FacturadorModels.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
namespace FacturadoBlazor.Service
{
    public class FacturaService
    {
        private readonly HttpClient _httpClient;
        public FacturaService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<List<FacturaVista>> ObtenerFacturaAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<List<FacturaVista>>>("https://localhost:7185/api/Factura/GetAllFacturas");
            return response?.Data ?? new List<FacturaVista>();
        }
        public async Task<List<VwFacturaResuman>> ResumenFacturas()
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<List<VwFacturaResuman>>>("https://localhost:7185/api/Factura/Vista1");
            return response?.Data ?? new List<VwFacturaResuman>();
        }
        public async Task<List<VwFacturaCliente>> FacturasClientes()
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<List<VwFacturaCliente>>>("https://localhost:7185/api/Factura/Vista2");
            return response?.Data ?? new List<VwFacturaCliente>();
        }
        public async Task<HttpResponseMessage> AddFactura(CreateFactura facturaVista)
        {
            return await _httpClient.PostAsJsonAsync("https://localhost:7185/api/Factura/CreateFactura", facturaVista);

        }
        public async Task<FacturaVista> UpdateFactura(FacturaVista facturacabezera, int id)
        {
            var response = await _httpClient.PutAsJsonAsync($"https://localhost:7185/api/Factura/PutUpdateFacturas/{id}", facturacabezera);

            if (response.IsSuccessStatusCode)
            {

                var result = await response.Content.ReadFromJsonAsync<ApiResponseviste>();
                return result.FacturaVista;
            }

            throw new Exception("Error al crear cliente: " + response.ReasonPhrase);
        }
        public async Task<FacturaVista> UpdateFacturaDDetalle(FacturaVista facturaDetalle)
        {
            var response = await _httpClient.PutAsJsonAsync("https://localhost:7185/api/Factura/UpdateFacturaDetalle", facturaDetalle);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<FacturaVista>();
                return result; // ✅ retornamos el resultado correctamente
            }

            return null; // en caso de error
        }


        public async Task<ApiResponseViste> BuscarDatos(DateTime fechaDesde, DateTime fechaHasta, int idCliente)
        {
            string url = $"https://localhost:7185/api/Factura/PorClienteMasVendido?fechaDesde={fechaDesde:yyyy-MM-dd}&fechaHasta={fechaHasta:yyyy-MM-dd}&idCliente={idCliente}";

            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ApiResponseViste>();
                return result!;
            }

            throw new Exception("Error al obtener datos: " + response.ReasonPhrase);
        }
        public async Task<List<FacturaDetalleCliente>> BuscarClienteFactura(DateTime fechaDesde, DateTime fechaHasta, int idCliente)
        {
            string url = $"https://localhost:7185/api/Factura/FactuClienteConDetalle?fechaDesde={fechaDesde:yyyy-MM-dd}&fechaHasta={fechaHasta:yyyy-MM-dd}&idCliente={idCliente}";

            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<List<FacturaDetalleCliente>>();
                return result;
            }

            throw new Exception("Error al obtener datos: " + response.ReasonPhrase);
        }
        public async Task<string> DeleteFactura(int id)
        {
            var response = await _httpClient.PatchAsJsonAsync(
                $"https://localhost:7185/api/Factura/PatchCancelinvoice/{id}",
                new { }
            );

            if (response.IsSuccessStatusCode)
            {
                return "Factura deshabilitada correctamente";
            }

            throw new Exception("Error al deshabilitar la factura: " + response.ReasonPhrase);
        }

        private class ApiResponse
        {
            public string message { get; set; }
            public CreateFactura Facturacreate { get; set; }
        }
        private class ApiResponseviste
        {
            public string message { get; set; }
            public FacturaVista FacturaVista { get; set; }
        }
        private class ApiResponsevistedetalle
        {
            public string message { get; set; }
            public FacturaDetalle FacturaDetalle { get; set; }
        }
    }
}
