using System.ComponentModel.DataAnnotations;

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
    }
}
