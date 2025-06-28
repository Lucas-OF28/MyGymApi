namespace MyGymApi.Api.DTOs
{
    public class AulaDto
    {
        public int Id { get; set; }
        public string Tipo { get; set; } = null!;
        public DateTime DataHora { get; set; }
        public int CapacidadeMaxima { get; set; }
    }
}
