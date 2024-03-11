using AutoMapper;
using Domain;
using Services.DTOs.DemoItemDTOs;

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
