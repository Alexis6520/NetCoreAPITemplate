using ApplicationServices.Commands.DemoItemCommands;
using ApplicationServices.Exceptions;
using DomainServices.Services;
using MediatR;

namespace ApplicationServices.Handlers.DemoItemHandlers
{
    public class UpdateDemoItemHandler(IUnitOfWork context) : IRequestHandler<UpdateDemoItemCommand>
    {
        private IUnitOfWork _context = context;

        public async Task Handle(UpdateDemoItemCommand request, CancellationToken cancellationToken)
        {
            var demoItem = await _context.DemoItems
                .FindAsync(x => x.Id == request.Id, cancellationToken);

            _ = demoItem ?? throw new NotFoundException("Artículo demo no encontrado.");
            demoItem.Name = request.Name;
            demoItem.Description = request.Description;
            demoItem.Price = request.Price;
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
