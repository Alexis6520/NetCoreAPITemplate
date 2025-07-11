using Application.ROP;
using Domain.Entities;
using Domain.Services;
using MediatR;
using System.Net;

namespace Application.Commands.Donuts.Create
{
    public class CreateDonutHandler(
        ApplicationDbContext dbContext) : IRequestHandler<CreateDonutCommand, Result<int>>
    {
        private readonly ApplicationDbContext _dbContext = dbContext;

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
            return Result<int>.Success(donut.Id, HttpStatusCode.Created);
        }
    }
}
