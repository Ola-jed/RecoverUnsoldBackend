using Microsoft.EntityFrameworkCore;
using RecoverUnsoldDomain.Data;

namespace RecoverUnsoldApi.Tests.Helpers;

public static class DbContextHelper
{
    public static DataContext GetDbContext()
    {
        return new DataContext(new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);
    }
}