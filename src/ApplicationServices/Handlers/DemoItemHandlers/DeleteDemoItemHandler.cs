using ApplicationServices.Commands.DemoItemCommands;
using ApplicationServices.Exceptions;
using DomainServices.Services;
using MediatR;

namespace ApplicationServices.Handlers.DemoItemHandlers
{
    public class DeleteDemoItemHandler(IUnitOfWork context) : IRequestHandler<DeleteDemoItemCommand>
    {
        private readonly IUnitOfWork _context = context;

        public async Task Handle(DeleteDemoItemCommand request, CancellationToken cancellationToken)
        {
            var demoItem = await _context.DemoItems
                .FindAsync(x => x.Id == request.Id, cancellationToken);

            _ = demoItem ?? throw new NotFoundException("Artículo demo no encontrado");
            await _context.DemoItems.DeleteAsync(demoItem, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
