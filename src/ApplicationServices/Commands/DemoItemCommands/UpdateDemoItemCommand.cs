using MediatR;

namespace ApplicationServices.Commands.DemoItemCommands
{
    public class UpdateDemoItemCommand : IRequest
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }
}
