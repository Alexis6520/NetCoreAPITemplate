﻿using AutoMapper;
using DomainServices.Common;
using DomainServices.Repositories;
using DomainServices.Services;
using System.Reflection;

namespace Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public UnitOfWork(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            SetRepositories();
        }

        public IDemoItemRepository DemoItems { get; set; }

        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        private void SetRepositories()
        {
            var propsInfo = GetType().GetProperties()
                .Where(x => x.PropertyType.GetInterfaces().Any(y => y.IsGenericType && y.GetGenericTypeDefinition() == typeof(IRepository<>)));

            var assembly = Assembly.GetExecutingAssembly();

            foreach (var propInfo in propsInfo)
            {
                var repoClass = assembly.GetTypes()
                    .Where(x => x.IsClass && x.IsAssignableTo(propInfo.PropertyType))
                    .FirstOrDefault();

                if (repoClass != null)
                {
                    object repoInstance = Activator.CreateInstance(repoClass, _dbContext, _mapper);
                    propInfo.SetValue(this, repoInstance);
                }
            }
        }
    }
}
