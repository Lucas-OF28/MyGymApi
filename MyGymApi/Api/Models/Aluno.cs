namespace MyGymApi.Api.Models
{
    public enum Plano
    {
        Mensal,
        Trimestral,
        Anual
    }
    public class Aluno
    {
        public int Id { get; set; }
        public string Nome { get; set; } = null!;
        public Plano Plano { get; set; }
        public ICollection<Agendamento> Agendamentos { get; set; } = new List<Agendamento>();
    }
}
