using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using FacturadorModels.Models;
using System.Net.Http;

namespace FacturadoBlazor.Service
{
    public class ArticuloService
    {
        private readonly HttpClient _httpClient;
        public ArticuloService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<List<Articulo>> ObtenerArticuloAsync()
        {
            // 👇 Cambiá la URL según tu API
            return await _httpClient.GetFromJsonAsync<List<Articulo>>("https://localhost:7185/api/Articulo");
        }
        public async Task<Articulo> AddArticuloAsync(Articulo articulo)
        {
            var response = await _httpClient.PostAsJsonAsync("https://localhost:7185/api/Articulo", articulo);

            if (response.IsSuccessStatusCode)
            {
                // Mapear la respuesta de tu API
                var result = await response.Content.ReadFromJsonAsync<ApiResponse>();
                return result.Articulo; // cliente creado
            }

            throw new Exception("Error al crear cliente: " + response.ReasonPhrase);
        }
        public async Task<Articulo> UpdateArticuloAsync(string cod, Articulo articulo)
        {
            var response = await _httpClient.PutAsJsonAsync(
      $"https://localhost:7185/api/Articulo/{cod}",
      articulo
  );

            if (response.IsSuccessStatusCode)
            {
                // Mapear la respuesta de tu API
                var result = await response.Content.ReadFromJsonAsync<ApiResponse>();
                return result.Articulo; // cliente creado
            }

            throw new Exception("Error al crear cliente: " + response.ReasonPhrase);
        }
        public async Task<Articulo> DesactivarArticulo(string cod)
        {
            // Enviar PATCH con body vacío
            var response = await _httpClient.PatchAsJsonAsync(
                $"https://localhost:7185/api/Articulo/DeleteArticulo/{cod}",
                new { } // body vacío necesario
            );

            if (response.IsSuccessStatusCode)
            {
                // Mapear la respuesta de tu API
                var result = await response.Content.ReadFromJsonAsync<ApiResponse>();
                return result.Articulo; // devuelve el artículo actualizado
            }

            throw new Exception("Error al deshabilitar artículo: " + response.ReasonPhrase);
        }

        private class ApiResponse
        {
            public string message { get; set; }
            public Articulo Articulo { get; set; }
        }
    }
}
