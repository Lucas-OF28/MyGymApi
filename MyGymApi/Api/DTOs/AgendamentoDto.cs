namespace MyGymApi.Api.DTOs
{
    public class AgendamentoDto
    {
        public int Id { get; set; }
        public string Aluno { get; set; } = null!;
        public string Aula { get; set; } = null!;
        public DateTime DataHora { get; set; }
    }
}
