using MediatR;

namespace Logic.Commands
{
    public class DemoItemCreateCommand(string name, decimal price) : IRequest
    {
        public string Name { get; set; } = name;
        public decimal Price { get; set; } = price;
    }
}
