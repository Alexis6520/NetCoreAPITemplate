using Application.RP;
using MediatR;

namespace Application.Commands.Donuts
{
    public class CreateDonutCommand : IRequest<Result<int>>
    {
        public string Name { get; set; } = String.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
    }
}
