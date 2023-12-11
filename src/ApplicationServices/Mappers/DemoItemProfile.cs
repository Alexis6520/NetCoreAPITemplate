using ApplicationServices.Commands.DemoItemCommands;
using AutoMapper;
using Domain.Entities;
using DomainServices.DTOs.DemoItemDTOs;

namespace ApplicationServices.Mappers
{
    public class DemoItemProfile : Profile
    {
        public DemoItemProfile()
        {
            CreateMap<CreateDemoItemCommand, DemoItem>();
            CreateMap<DemoItem, DemoItemSearchDTO>();
        }
    }
}
