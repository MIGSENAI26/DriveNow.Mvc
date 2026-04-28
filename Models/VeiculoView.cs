using System.ComponentModel.DataAnnotations.Schema;

namespace DriveNow.MVC.Models
{
    public class VeiculoView
    {
        public int Id { get; set; }
        public string? Modelo { get; set; }
        public string? Placa { get; set; }
        public double ValorDiaria { get; set; }
        public int AgenciaId { get; set; }
        public string? AgenciaNome { get; set; }

        public string? FotoUrlVeiculo { get; set; } // Propriedade para armazenar a URL da foto do pet no banco de dados

        [NotMapped] // This property is not mapped to the database
        public IFormFile FotoUploadVeiculo { get; set; } // Aparece na View, mas não é armazenada no banco de dados

    }
}
