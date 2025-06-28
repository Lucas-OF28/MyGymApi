namespace MyGymApi.Api.DTOs
{
    public class CreateAulaDto
    {
        public string Tipo { get; set; } = null!;
        public DateTime DataHora { get; set; }
        public int CapacidadeMaxima { get; set; }
    }
}
