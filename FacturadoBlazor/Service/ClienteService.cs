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
        return await _httpClient.GetFromJsonAsync<List<Cliente>>("https://localhost:7185/api/Clientes/GetAllclientes");
    }
    public async Task<HttpResponseMessage> CrearClienteAsync(Cliente cliente)
    {
        return await _httpClient.PostAsJsonAsync("https://localhost:7185/api/Clientes/AddClientes", cliente);
    }

    public async Task<Cliente> EditarClienteAsync(int id, Cliente cliente)
    {
        var response = await _httpClient.PutAsJsonAsync($"https://localhost:7185/api/Clientes/UpdateClientes/{id}", cliente);

        if (!response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
            throw new Exception(result?["message"] ?? "Error desconocido al editar cliente");
        }

        return await response.Content.ReadFromJsonAsync<Cliente>();
    }

    public async Task<bool> DesactivarCliente(int id)
    {
        var response = await _httpClient.PatchAsync(
            $"https://localhost:7185/api/Clientes/DeleteCliente/{id}", // sin "DeleteCliente"
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
