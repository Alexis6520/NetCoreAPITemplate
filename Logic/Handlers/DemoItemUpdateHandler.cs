using Logic.Commands;
using Logic.Exceptions;
using MediatR;
using Services;

namespace Logic.Handlers
{
    public class DemoItemUpdateHandler(IUnitOfWork unitOfWork) : IRequestHandler<DemoItemUpdateCommand>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task Handle(DemoItemUpdateCommand request, CancellationToken cancellationToken)
        {
            /*
                Código con vainas lógicas y reglas de negocio 
             */

            var demoItem = await _unitOfWork.DemoItems.GetAsync([request.Id], cancellationToken);
            _ = demoItem ?? throw new NotFoundException("Articulo no encontrado");
            demoItem.Name = request.Name;
            demoItem.Price = request.Price;
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
