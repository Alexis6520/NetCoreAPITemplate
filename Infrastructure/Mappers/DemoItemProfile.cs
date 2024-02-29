using AutoMapper;
using Domain;
using Services.DTOs;

namespace Infrastructure.Mappers
{
    public class DemoItemProfile : Profile
    {
        public DemoItemProfile()
        {
            CreateMap<DemoItem, DemoItemDTO>();
        }
    }
}
