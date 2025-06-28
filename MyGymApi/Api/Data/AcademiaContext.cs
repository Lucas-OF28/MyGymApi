using Microsoft.EntityFrameworkCore;
using MyGymApi.Api.Models;

namespace MyGymApi.Api.Data
{
    public class AcademiaContext : DbContext
    {
        public AcademiaContext(DbContextOptions<AcademiaContext> options) : base(options) { }

        public DbSet<Aluno> Alunos => Set<Aluno>();
        public DbSet<Aula> Aulas => Set<Aula>();
        public DbSet<Agendamento> Agendamentos => Set<Agendamento>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Agendamento>()
                .HasIndex(a => new { a.AlunoId, a.AulaId })
                .IsUnique();
        }
    }
}
