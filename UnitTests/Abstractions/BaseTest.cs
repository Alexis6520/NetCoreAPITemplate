using Microsoft.EntityFrameworkCore;
using Persistence;

namespace UnitTests.Abstractions;

public abstract class BaseTest
{
    private ApplicationDbContext? _dbContext;

#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de agregar el modificador "required" o declararlo como un valor que acepta valores NULL.
    public TestContext TestContext { get; set; }
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de agregar el modificador "required" o declararlo como un valor que acepta valores NULL.

    protected ApplicationDbContext DbContext
    {
        get
        {
            if (_dbContext is not null) return _dbContext;

            var opts = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString("N"))
                .Options;

            return _dbContext = new ApplicationDbContext(opts);
        }
    }
}
