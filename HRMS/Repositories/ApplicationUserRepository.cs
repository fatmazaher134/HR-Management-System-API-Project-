using HRMS.Data;

namespace HRMS.Repositories;

public class ApplicationUserRepository : GenericRepository<ApplicationUser>, IApplicationUserRepository
{
    public ApplicationUserRepository(ApplicationDbContext context) : base(context)
    {
    }
}
