using ApplicationServices.Commands.DemoItemCommands;
using AutoMapper;
using Domain.Entities;
using DomainServices.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ApplicationServices.Handlers.DemoItemHandlers
{

    public class CreateDemoItemHandler(IUnitOfWork context, IMapper mapper, ILogger<CreateDemoItemHandler> logger) : IRequestHandler<CreateDemoItemCommand, int>
    {
        private readonly IUnitOfWork _context = context;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<CreateDemoItemHandler> _logger = logger;

        public async Task<int> Handle(CreateDemoItemCommand request, CancellationToken cancellationToken)
        {
            var newItem = _mapper.Map<DemoItem>(request);
            await _context.DemoItems.AddAsync(newItem, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            var message = $"Nuevo producto: {newItem.Id}.";
            _logger.LogInformation("{message}", message);
            return newItem.Id;
        }
    }
}
