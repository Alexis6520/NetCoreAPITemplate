using Logic.Commands;
using Logic.Exceptions;
using MediatR;
using Services;

namespace Logic.Handlers
{
    public class DemoItemDeleteHandler(IUnitOfWork unitOfWork) : IRequestHandler<DemoItemDeleteCommand>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task Handle(DemoItemDeleteCommand request, CancellationToken cancellationToken)
        {
            /*
                Código con vainas lógicas y reglas de negocio 
             */

            var demoItem = await _unitOfWork.DemoItems.GetAsync([request.Id], cancellationToken);
            _ = demoItem ?? throw new NotFoundException("Articulo demo no encontrado");
            _unitOfWork.DemoItems.Remove(demoItem);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
