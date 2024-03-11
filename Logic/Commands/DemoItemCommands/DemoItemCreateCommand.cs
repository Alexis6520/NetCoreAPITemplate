using MediatR;

namespace Logic.Commands.DemoItemCommands
{
    public class DemoItemCreateCommand(string name, decimal price) : IRequest
    {
        public string Name { get; set; } = name;
        public decimal Price { get; set; } = price;
    }
}
