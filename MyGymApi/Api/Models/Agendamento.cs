namespace MyGymApi.Api.Models
{
    public class Agendamento
    {
        public int Id { get; set; }

        public int AlunoId { get; set; }
        public Aluno Aluno { get; set; } = null!;

        public int AulaId { get; set; }
        public Aula Aula { get; set; } = null!;
    }

}
