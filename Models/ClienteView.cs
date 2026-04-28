using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DriveNow.MVC.Models
{
    public class ClienteView
    {
        public int Id { get; set; }
        [Required] public string? Nome { get; set; }
        [Required(ErrorMessage = "E-mail obrigatório")]
        [EmailAddress(ErrorMessage = "E-mail inválido")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "CPF obrigatório")]
        public string? Cpf { get; set; }

        public string? FotoUrlCliente { get; set; } // Propriedade para armazenar a URL da foto do pet no banco de dados

        [NotMapped] // This property is not mapped to the database
        public IFormFile FotoUploadCliente { get; set; } // Aparece na View, mas não é armazenada no banco

    }
}
