using Domain;
using Logic.Commands;
using MediatR;
using Services;

namespace Logic.Handlers
{
    public class DemoItemCreateHandler(IUnitOfWork unitOfWork) : IRequestHandler<DemoItemCreateCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<int> Handle(DemoItemCreateCommand request, CancellationToken cancellationToken)
        {
            /*
                Código con vainas lógicas y reglas de negocio 
             */

            var demoItem = new DemoItem(request.Name, request.Price);
            await _unitOfWork.Items.AddAsync(demoItem, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return demoItem.Id;
        }
    }
}
