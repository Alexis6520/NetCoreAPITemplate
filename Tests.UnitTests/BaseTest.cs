using Microsoft.Extensions.Logging;
using Moq;
using Services;

namespace Infrastructure.UnitTests
{
    public abstract class BaseTest<THandler> where THandler : class
    {
        protected readonly Mock<ILogger<THandler>> _loggerMock = new();
        protected readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    }
}
