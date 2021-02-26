using Authenticator_API.Data;
using Microsoft.EntityFrameworkCore;

namespace Authenticator_Testes.DataAcessTest
{
    class SetupTestDb
    {
        private readonly UserContext _context;
        public SetupTestDb()
        {
            var connectionString = "Server=(localdb)\\MSSQLLocalDB; Database = TestUsersAuth; Trusted_Connection = True; MultipleActiveResultSets = true";
            var builder = new DbContextOptionsBuilder<UserContext>();
            builder.UseSqlServer(connectionString);
            var options = builder.Options;

            _context = new UserContext(options);

            DbTestInitializer.Initialize(_context);
        }

        public UserContext GetContext()
        {
            return _context;
        }
    }
}
