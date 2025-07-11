using Application.ROP;
using Domain.Entities;
using Domain.Services;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Application.Commands.Donuts.Create
{
    public class CreateDonutHandler(
        ApplicationDbContext dbContext,
        ILogger<CreateDonutHandler> logger) : IRequestHandler<CreateDonutCommand, Result<int>>
    {
        private readonly ApplicationDbContext _dbContext = dbContext;
        private readonly ILogger<CreateDonutHandler> _logger = logger;

        public async Task<Result<int>> Handle(CreateDonutCommand request, CancellationToken cancellationToken)
        {
            var donut = new Donut
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price
            };

            _dbContext.Donuts.Add(donut);
            await _dbContext.SaveChangesAsync(default);
            _logger.LogInformation("Nueva dona con Id: {Id}", donut.Id);
            return Result<int>.Success(donut.Id, HttpStatusCode.Created);
        }
    }
}
