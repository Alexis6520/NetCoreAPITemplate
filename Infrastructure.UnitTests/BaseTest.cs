using Microsoft.Extensions.Logging;
using Moq;
using Services;
using Services.Repositories;

namespace Infrastructure.UnitTests
{
    public abstract class BaseTest<THandler> where THandler : class
    {
        protected readonly Mock<ILogger<THandler>> _loggerMock = new();
        protected readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
        protected readonly Mock<IDemoItemRepository> _demoItemRepoMock = new();
    }
}
