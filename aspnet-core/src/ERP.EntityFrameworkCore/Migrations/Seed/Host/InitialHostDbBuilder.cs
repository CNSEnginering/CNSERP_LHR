using ERP.EntityFrameworkCore;

namespace ERP.Migrations.Seed.Host
{
    public class InitialHostDbBuilder
    {
        private readonly ERPDbContext _context;

        public InitialHostDbBuilder(ERPDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            new DefaultEditionCreator(_context).Create();
            new DefaultLanguagesCreator(_context).Create();
            new HostRoleAndUserCreator(_context).Create();
            new DefaultSettingsCreator(_context).Create();

            _context.SaveChanges();
        }
    }
}
