using MediatR;

namespace Logic.Commands.DemoItemCommands
{
    public class DemoItemDeleteCommand : IRequest
    {
        public int Id { get; set; }
    }
}
