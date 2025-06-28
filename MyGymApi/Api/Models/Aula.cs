namespace MyGymApi.Api.Models
{
    public class Aula
    {
        public int Id { get; set; }
        public string Tipo { get; set; } = null!;
        public DateTime DataHora { get; set; }
        public int CapacidadeMaxima { get; set; }
        public ICollection<Agendamento> Agendamentos { get; set; } = new List<Agendamento>();
    }

}
