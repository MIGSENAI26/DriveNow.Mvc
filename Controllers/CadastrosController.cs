using Microsoft.AspNetCore.Mvc;
using DriveNow.MVC.Models;

public class CadastroController : Controller
{
    private readonly HttpClient _httpClient;
    
    private readonly string _baseUrl = "https://localhost:7189";

    public CadastroController(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient();
    }

    // --- CLIENTES ---
    public IActionResult NovoCliente() => View();

    //[HttpPost]
    //public async Task<IActionResult> NovoCliente(ClienteView cliente)
    //{
    //    var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}clientes", cliente);
    //    if (response.IsSuccessStatusCode) return RedirectToAction("Sucesso");
    //    return View(cliente);
    //}

    //// --- VEÍCULOS ---
    //public IActionResult NovoVeiculo() => View();

    //[HttpPost]
    //public async Task<IActionResult> NovoVeiculo(VeiculoView veiculo)
    //{
    //    var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}veiculos", veiculo);
    //    if (response.IsSuccessStatusCode) return RedirectToAction("Sucesso");
    //    return View(veiculo);
    //}

    //// --- LOCAÇÕES ---
    //public IActionResult NovaLocacao() => View();

    //[HttpPost]
    //public async Task<IActionResult> NovaLocacao(LocacaoView locacao)
    //{
    //    var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}locacoes", locacao);
    //    if (response.IsSuccessStatusCode) return RedirectToAction("Sucesso");
    //    return View(locacao);
    //}

    //public IActionResult Sucesso() => Content("Cadastro realizado com sucesso!");

    [HttpPost]
    public async Task<IActionResult> NovoCliente(ClienteView cliente)
    {
        if (!ModelState.IsValid) return View(cliente);

        using (var content = new MultipartFormDataContent())
        {
            // Adiciona os campos de texto
            content.Add(new StringContent(cliente.Nome ?? ""), "Nome");
            content.Add(new StringContent(cliente.Email ?? ""), "Email");
            content.Add(new StringContent(cliente.Cpf.ToString()), "Cpf");

            // Adiciona o arquivo (Foto)
            if (cliente.FotoUploadCliente != null)
            {
                var fileStream = cliente.FotoUploadCliente.OpenReadStream();
                var fileContent = new StreamContent(fileStream);
                fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(cliente.FotoUploadCliente.ContentType);
                content.Add(fileContent, "FotoUploadCliente", cliente.FotoUploadCliente.FileName);
            }

            var response = await _httpClient.PostAsync($"{_baseUrl}/clientes", content);

            if (response.IsSuccessStatusCode) return RedirectToAction("Sucesso");
        }

        return View(cliente);
    }

    [HttpPost]
    public async Task<IActionResult> NovoVeiculo(VeiculoView veiculo)
    {
        if (!ModelState.IsValid) return View(veiculo);

        using (var content = new MultipartFormDataContent())
        {
            content.Add(new StringContent(veiculo.Modelo ?? ""), "Modelo");
            content.Add(new StringContent(veiculo.Placa ?? ""), "Placa");
            content.Add(new StringContent(veiculo.ValorDiaria.ToString()), "ValorDiaria");
            content.Add(new StringContent(veiculo.AgenciaNome ?? ""), "AgenciaNome");

            if (veiculo.FotoUploadVeiculo != null)
            {
                var fileStream = veiculo.FotoUploadVeiculo.OpenReadStream();
                var fileContent = new StreamContent(fileStream);
                fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(veiculo.FotoUploadVeiculo.ContentType);
                content.Add(fileContent, "FotoUploadVeiculo", veiculo.FotoUploadVeiculo.FileName);
            }

            var response = await _httpClient.PostAsync($"{_baseUrl}/veiculos", content);

            if (response.IsSuccessStatusCode) return RedirectToAction("Sucesso");
        }

        return View(veiculo);
    }
}