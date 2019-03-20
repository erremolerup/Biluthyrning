using BiluthyrningAB.Data;


namespace BiluthyrningAB.Controllers
{
    public class EntityFrameworkRepository : IEntityFrameworkRepository
    {
        private readonly AppDbContext _context;

        public EntityFrameworkRepository(AppDbContext context)
        {
            _context = context;
        }
        public async void SaveChangesAsync()
        {
            await _context.SaveChangesAsync();

        }
    }
}
