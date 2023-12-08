using ApplicationServices.Commands.DemoItemCommands;
using AutoMapper;
using Domain.Entities;

namespace ApplicationServices.Mappers
{
    public class DemoItemProfile : Profile
    {
        public DemoItemProfile()
        {
            CreateMap<CreateDemoItemCommand, DemoItem>();
        }
    }
}
