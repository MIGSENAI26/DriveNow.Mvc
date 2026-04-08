namespace DriveNow.MVC.Models
{
    public class LocacaoView
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public string? ClienteNome { get; set; }
        public int VeiculoId { get; set; }
        public string? VeiculoIdentificador { get; set; }
        public DateTime DataRetirada { get; set; }
        public DateTime DataDevolucao { get; set; }
        public double ValorTotal { get; set; }
    }
}
