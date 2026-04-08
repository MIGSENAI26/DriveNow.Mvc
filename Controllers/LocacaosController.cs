using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using DriveNow.MVC.Models;
using System.Text.Json;
using System.Text;

namespace DriveNow.MVC.Controllers
{
    public class LocacaosController : Controller
    {
        private readonly HttpClient _httpClient;

        public LocacaosController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("DriveNow.API");
        }

        // GET: Locacaos
        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetAsync("locacoes");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var locacoes = JsonSerializer.Deserialize<List<LocacaoView>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return View(locacoes);
            }
            return View(new List<LocacaoView>());
        }

        // GET: Locacaos/Create
        public async Task<IActionResult> Create()
        {
            // Carregando dados para os Dropdowns diretamente da API
            var clientes = await FetchDataFromApi<List<ClienteView>>("clientes");
            var veiculos = await FetchDataFromApi<List<VeiculoView>>("veiculos");

            ViewBag.ClienteId = new SelectList(clientes, "Id", "Nome");
            ViewBag.VeiculoId = new SelectList(veiculos, "Id", "Modelo");

            return View();
        }

        // POST: Locacaos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LocacaoView locacao)
        {
            var json = new StringContent(JsonSerializer.Serialize(locacao), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("locacaos", json);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            // Caso a API recuse (Ex: Veículo indisponível)
            var erro = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, "Erro na reserva: " + erro);

            // Recarrega os combos para a view não quebrar
            await Create();
            return View(locacao);
        }

        // GET: Locacaos/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpClient.GetAsync($"locacaos/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();

            var content = await response.Content.ReadAsStringAsync();
            var locacao = JsonSerializer.Deserialize<LocacaoView>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return View(locacao);
        }

        // POST: Locacaos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _httpClient.DeleteAsync($"locacaos/{id}");
            return RedirectToAction(nameof(Index));
        }

        // Método auxiliar para evitar repetição de código ao buscar listas para Dropdowns
        private async Task<T> FetchDataFromApi<T>(string endpoint) where T : new()
        {
            var response = await _httpClient.GetAsync(endpoint);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            return new T();
        }
    }
}
