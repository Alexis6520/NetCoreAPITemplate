﻿using Domain;
using Logic.Commands.DemoItemCommands;
using MediatR;
using Microsoft.Extensions.Logging;
using Services;

namespace Logic.Handlers.DemoItemHandlers
{
    public class DemoItemCreateHandler(IUnitOfWork unitOfWork, ILogger<DemoItemCreateHandler> logger) : IRequestHandler<DemoItemCreateCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ILogger<DemoItemCreateHandler> _logger = logger;

        public async Task<int> Handle(DemoItemCreateCommand request, CancellationToken cancellationToken)
        {
            /*
                Código con vainas lógicas y reglas de negocio 
             */

            var demoItem = new DemoItem(request.Name, request.Price);
            await _unitOfWork.DemoItems.AddAsync(demoItem, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Nuevo artículo demo: {Id}", demoItem.Id);
            return demoItem.Id;
        }
    }
}
