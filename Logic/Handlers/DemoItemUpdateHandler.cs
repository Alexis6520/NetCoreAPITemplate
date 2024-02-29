using Logic.Commands;
using Logic.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;
using Services;

namespace Logic.Handlers
{
    public class DemoItemUpdateHandler(IUnitOfWork unitOfWork, ILogger<DemoItemUpdateHandler> logger) : IRequestHandler<DemoItemUpdateCommand>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ILogger<DemoItemUpdateHandler> _logger = logger;

        public async Task Handle(DemoItemUpdateCommand request, CancellationToken cancellationToken)
        {
            /*
                Código con vainas lógicas y reglas de negocio 
             */

            var demoItem = await _unitOfWork.DemoItems.GetAsync([request.Id], cancellationToken);
            _ = demoItem ?? throw new NotFoundException($"Articulo {request.Id} no encontrado");
            demoItem.Name = request.Name;
            demoItem.Price = request.Price;
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Artículo {Id} actualizado", demoItem.Id);
        }
    }
}
