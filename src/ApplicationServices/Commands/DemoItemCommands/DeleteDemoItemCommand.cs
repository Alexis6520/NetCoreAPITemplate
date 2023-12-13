using MediatR;

namespace ApplicationServices.Commands.DemoItemCommands
{
    public class DeleteDemoItemCommand : IRequest
    {
        public int Id { get; set; }
    }
}
