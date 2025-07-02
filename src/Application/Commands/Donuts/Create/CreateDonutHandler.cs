using Application.ROP;
using Application.Utils.Extensions;
using Domain.Entities;
using Domain.Services;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Application.Commands.Donuts.Create
{
    public class CreateDonutHandler(
        ApplicationDbContext dbContext,
        ILogger<CreateDonutHandler> logger,
        IValidator<CreateDonutCommand> validator) : IRequestHandler<CreateDonutCommand, Result<int>>
    {
        private readonly ApplicationDbContext _dbContext = dbContext;
        private readonly ILogger<CreateDonutHandler> _logger = logger;
        private readonly IValidator<CreateDonutCommand> _validator = validator;

        public async Task<Result<int>> Handle(CreateDonutCommand request, CancellationToken cancellationToken)
        {
            return await _validator.ValidateAndMapAsync(request)
                .BindAsync(CheckNameAvailability)
                .MapAsync(SaveDonut)
                .WithStatusCodeAsync(HttpStatusCode.Created);
        }

        private async Task<Result<CreateDonutCommand>> CheckNameAvailability(CreateDonutCommand request)
        {
            _logger.LogInformation("Comprobando disponibilidad del nombre: {Name}...", request.Name);
            bool nameAvailable = !await _dbContext.Donuts
                .AnyAsync(d => d.Name == request.Name);

            if (!nameAvailable)
            {
                _logger.LogWarning("El nombre '{Name}' no está disponible.", request.Name);
                return Result<CreateDonutCommand>.Failure(HttpStatusCode.Conflict, Errors.DONUT_NAME_NOT_AVAILABLE);
            }

            _logger.LogInformation("Nombre '{Name}' disponible.", request.Name);
            return Result<CreateDonutCommand>.Success(request);
        }

        private async Task<int> SaveDonut(CreateDonutCommand request)
        {
            var donut = new Donut
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price
            };

            _dbContext.Donuts.Add(donut);
            _logger.LogInformation("Guardando donita: {Name}...", donut.Name);
            await _dbContext.SaveChangesAsync();
            _logger.LogInformation("Donita guardada con ID: {Id}", donut.Id);
            return donut.Id;
        }
    }
}
