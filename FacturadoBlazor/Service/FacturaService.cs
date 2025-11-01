using Azure;
using FacturadorModels.Models;
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
        public async Task<CreateFactura> AddFactura(CreateFactura facturaVista)
        {
            var response = await _httpClient.PostAsJsonAsync("https://localhost:7185/api/Factura", facturaVista);

            if (response.IsSuccessStatusCode)
            {
                
                var result = await response.Content.ReadFromJsonAsync<ApiResponse>();
                return result.Facturacreate;
            }
            throw new Exception("Error al crear cliente: " + response.ReasonPhrase);
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
    }
}
