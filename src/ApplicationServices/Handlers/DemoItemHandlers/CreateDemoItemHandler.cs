using ApplicationServices.Commands.DemoItemCommands;
using AutoMapper;
using Domain.Entities;
using DomainServices.Services;
using MediatR;

namespace ApplicationServices.Handlers.DemoItemHandlers
{

    public class CreateDemoItemHandler(IUnitOfWork context, IMapper mapper) : IRequestHandler<CreateDemoItemCommand, int>
    {
        private readonly IUnitOfWork _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<int> Handle(CreateDemoItemCommand request, CancellationToken cancellationToken)
        {
            var newItem = _mapper.Map<DemoItem>(request);
            await _context.DemoItems.AddAsync(newItem, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return newItem.Id;
        }
    }
}
