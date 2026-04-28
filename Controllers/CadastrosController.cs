using Microsoft.AspNetCore.Mvc;
using DriveNow.MVC.Models;
public class CadastrosController : Controller
{
    private readonly IHttpClientFactory _clientFactory;

    public CadastrosController(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }

    public IActionResult Index()
    {
        return View();
    }

    // --- CREATE VEÍCULO (Com Upload) ---
    [HttpPost]
    public async Task<IActionResult> NovoVeiculo(VeiculoView model)
    {
        var client = _clientFactory.CreateClient("DriveNow.API");
        using var content = new MultipartFormDataContent();

        // Campos de texto (precisam ter o mesmo nome que a API espera no objeto)
        content.Add(new StringContent(model.Modelo ?? ""), "Modelo");
        content.Add(new StringContent(model.Placa ?? ""), "Placa");
        content.Add(new StringContent(model.ValorDiaria.ToString()), "ValorDiaria");
        content.Add(new StringContent(model.AgenciaId.ToString()), "AgenciaId");

        // Envio do arquivo binário
        if (model.FotoUploadVeiculo != null)
        {
            var stream = model.FotoUploadVeiculo.OpenReadStream();
            var fileContent = new StreamContent(stream);
            content.Add(fileContent, "FotoUploadVeiculo", model.FotoUploadVeiculo.FileName);
        }

        var response = await client.PostAsync("api/veiculos", content);

        if (response.IsSuccessStatusCode) return RedirectToAction("Index", "Home");

        return View(model);
    }

    // --- CREATE CLIENTE (Com Upload) ---
    [HttpPost]
    public async Task<IActionResult> NovoCliente(ClienteView model)
    {
        var client = _clientFactory.CreateClient("DriveNow.API");
        using var content = new MultipartFormDataContent();

        content.Add(new StringContent(model.Nome ?? ""), "Nome");
        content.Add(new StringContent(model.Email ?? ""), "Email");
        content.Add(new StringContent(model.Cpf ?? ""), "Cpf");

        if (model.FotoUploadCliente != null)
        {
            var stream = model.FotoUploadCliente.OpenReadStream();
            var fileContent = new StreamContent(stream);
            content.Add(fileContent, "FotoUploadCliente", model.FotoUploadCliente.FileName);
        }

        var response = await client.PostAsync("api/clientes", content);

        if (response.IsSuccessStatusCode) return RedirectToAction("Index", "Home");

        return View(model);
    }
}