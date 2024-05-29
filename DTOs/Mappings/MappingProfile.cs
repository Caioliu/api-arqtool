using AutoMapper;
using caiobadev_api_arqtool.Models;

namespace caiobadev_api_arqtool.DTOs.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<DespesaMensal, DespesaMensalDto>().ReverseMap();
            CreateMap<ValorIdealHoraTrabalho, ValorIdealHoraTrabalhoDto>().ReverseMap();
            CreateMap<Etapa, EtapaDTO>().ReverseMap();
        }
        
    }
}
