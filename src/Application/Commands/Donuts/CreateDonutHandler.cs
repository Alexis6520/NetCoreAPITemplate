using Application.RP;
using Domain.Entities;
using Domain.Services;
using MediatR;
using System.Net;

namespace Application.Commands.Donuts
{
    public class CreateDonutHandler(AppDbContext dbContext) : IRequestHandler<CreateDonutCommand, Result<int>>
    {
        private readonly AppDbContext _dbContext = dbContext;

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
            return new(donut.Id, HttpStatusCode.Created);
        }
    }
}
