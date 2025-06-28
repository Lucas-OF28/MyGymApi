using AutoMapper;
using MyGymApi.Api.DTOs;
using MyGymApi.Api.Models;

namespace MyGymApi.Api.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Aula, AulaDto>();
        CreateMap<CreateAulaDto, Aula>();

        CreateMap<Aluno, AlunoDto>()
            .ForMember(dest => dest.Plano, opt => opt.MapFrom(src => src.Plano.ToString()));
        CreateMap<CreateAlunoDto, Aluno>()
            .ForMember(dest => dest.Plano, opt => opt.MapFrom(src => Enum.Parse<Plano>(src.Plano, true)));

        CreateMap<Agendamento, AgendamentoDto>()
            .ForMember(dest => dest.Aluno, opt => opt.MapFrom(src => src.Aluno.Nome))
            .ForMember(dest => dest.Aula, opt => opt.MapFrom(src => src.Aula.Tipo))
            .ForMember(dest => dest.DataHora, opt => opt.MapFrom(src => src.Aula.DataHora));
    }
}
