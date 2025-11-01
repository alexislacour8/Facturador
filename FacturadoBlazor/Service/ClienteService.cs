using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using FacturadorModels.Models;
public class ClienteService
{
    private readonly HttpClient _httpClient;

    public ClienteService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<Cliente>> ObtenerClientesAsync()
    {
        // 👇 Cambiá la URL según tu API
        return await _httpClient.GetFromJsonAsync<List<Cliente>>("https://localhost:7185/api/Clientes");
    }
    public async Task<Cliente> CrearClienteAsync(Cliente cliente)
    {
        var response = await _httpClient.PostAsJsonAsync("https://localhost:7185/api/Clientes", cliente);

        if (response.IsSuccessStatusCode)
        {
            // Mapear la respuesta de tu API
            var result = await response.Content.ReadFromJsonAsync<ApiResponse>();
            return result.client; // cliente creado
        }

        throw new Exception("Error al crear cliente: " + response.ReasonPhrase);
    }
    public async Task<Cliente> EditarCliente(int id, Cliente cliente)
    {
        // Serializamos el objeto a JSON
        var response = await _httpClient.PutAsJsonAsync($"https://localhost:7185/api/Clientes/{id}", cliente);

        if (response.IsSuccessStatusCode)
        {
            // Leemos la respuesta como Cliente directamente (si tu API devuelve el cliente actualizado)
            var clienteActualizado = await response.Content.ReadFromJsonAsync<Cliente>();
            return clienteActualizado!;
        }

        throw new Exception($"Error al editar cliente (ID {id}): {response.ReasonPhrase}");
    }
    public async Task<bool> DesactivarCliente(int id)
    {
        var response = await _httpClient.PatchAsync(
            $"https://localhost:7185/api/Clientes/{id}", // sin "DeleteCliente"
            null
        );

        if (response.IsSuccessStatusCode)
        {
            // solo devuelvo true si todo salió bien
            return true;
        }

        var error = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
        throw new Exception(error?["message"] ?? "Error al desactivar cliente");
    }


    private class ApiResponse
    {
        public string message { get; set; }
        public Cliente client { get; set; }
    }
}
