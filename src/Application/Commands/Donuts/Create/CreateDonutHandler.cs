using Application.ROP;
using Domain.Entities;
using Domain.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Commands.Donuts.Create
{
    public class CreateDonutHandler(ApplicationDbContext dbContext) : IRequestHandler<CreateDonutCommand, Result<int>>
    {
        private readonly ApplicationDbContext _dbContext = dbContext;

        public async Task<Result<int>> Handle(CreateDonutCommand request, CancellationToken cancellationToken)
        {
            return await CheckNameAvailability(request.Name)
                .MapAsync(_ => Task.FromResult(request))
                .MapAsync(SaveDonut);
        }

        private async Task<Result<Unity>> CheckNameAvailability(string name)
        {
            bool nameAvailable = !await _dbContext.Donuts
                .AnyAsync(d => d.Name == name);

            if (!nameAvailable) return Result<Unity>.Failure(Errors.DONUT_NAME_NOT_AVAILABLE);
            return Result<Unity>.Success();
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
            await _dbContext.SaveChangesAsync();
            return donut.Id;
        }
    }
}
