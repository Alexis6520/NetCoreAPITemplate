using Logic.Commands;
using Logic.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;
using Services;

namespace Logic.Handlers
{
    public class DemoItemDeleteHandler(IUnitOfWork unitOfWork, ILogger<DemoItemDeleteHandler> logger) : IRequestHandler<DemoItemDeleteCommand>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ILogger<DemoItemDeleteHandler> _logger = logger;

        public async Task Handle(DemoItemDeleteCommand request, CancellationToken cancellationToken)
        {
            /*
                Código con vainas lógicas y reglas de negocio 
             */

            var demoItem = await _unitOfWork.DemoItems.GetAsync([request.Id], cancellationToken);
            _ = demoItem ?? throw new NotFoundException($"Articulo demo {request.Id} no encontrado");
            _unitOfWork.DemoItems.Remove(demoItem);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Artículo demo {Id} eliminado", demoItem.Id);
        }
    }
}
