using Application.RP;
using Domain.Entities;
using Domain.Services;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Application.Commands.Donuts
{
    public class CreateDonutHandler(AppDbContext dbContext, ILogger<CreateDonutHandler> logger) : IRequestHandler<CreateDonutCommand, Result<int>>
    {
        private readonly AppDbContext _dbContext = dbContext;
        private readonly ILogger<CreateDonutHandler> _logger = logger;

        public async Task<Result<int>> Handle(CreateDonutCommand request, CancellationToken cancellationToken)
        {
            var donut = new Donut
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
            };

            _dbContext.Donuts.Add(donut);
            await _dbContext.SaveChangesAsync(default);
            _logger.LogInformation("Dona {Id} creada", donut.Id);
            return new(donut.Id, HttpStatusCode.Created);
        }
    }
}
