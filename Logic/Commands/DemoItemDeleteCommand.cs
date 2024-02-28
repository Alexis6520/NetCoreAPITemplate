using MediatR;

namespace Logic.Commands
{
    public class DemoItemDeleteCommand : IRequest
    {
        public int Id { get; set; }
    }
}
