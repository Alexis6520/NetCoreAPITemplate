using Application.ROP;
using Domain.Entities;
using Domain.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Application.Commands.Donuts.Create
{
    public class CreateDonutHandler(ApplicationDbContext dbContext) : IRequestHandler<CreateDonutCommand, Result<int>>
    {
        private readonly ApplicationDbContext _dbContext = dbContext;

        public async Task<Result<int>> Handle(CreateDonutCommand request, CancellationToken cancellationToken)
        {
            return await CheckNameAvailability(request)
                .MapAsync(SaveDonut)
                .WithStatusCodeAsync(HttpStatusCode.Created);
        }

        private async Task<Result<CreateDonutCommand>> CheckNameAvailability(CreateDonutCommand request)
        {
            bool nameAvailable = !await _dbContext.Donuts
                .AnyAsync(d => d.Name == request.Name);

            if (!nameAvailable)
                return Result<CreateDonutCommand>.Failure(HttpStatusCode.Conflict, Errors.DONUT_NAME_NOT_AVAILABLE);

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
            await _dbContext.SaveChangesAsync();
            return donut.Id;
        }
    }
}
