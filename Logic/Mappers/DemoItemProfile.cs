using AutoMapper;
using Domain;
using Services.DTOs;

namespace Logic.Mappers
{
    public class DemoItemProfile : Profile
    {
        public DemoItemProfile()
        {
            CreateMap<DemoItem, DemoItemDTO>();
        }
    }
}
