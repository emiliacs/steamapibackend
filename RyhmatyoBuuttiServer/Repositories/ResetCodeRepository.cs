using RyhmatyoBuuttiServer.Models;

namespace RyhmatyoBuuttiServer.Repositories
{
    public class ResetCodeRepository : IResetCodeRepository
    {
        private readonly DataContext _context;

        public ResetCodeRepository(DataContext context)
        {
            _context = context;
        }

        public PasswordResetCode findResetCodeByUserId(long id)
        {
            return _context.ResetCodes.Find(id);
        }

        public void AddResetCode(PasswordResetCode prc)
        {
            _context.ResetCodes.Add(prc);
            _context.SaveChanges();
        }
        
        public void UpdateResetCode(PasswordResetCode prc)
        {
            _context.ResetCodes.Update(prc);
            _context.SaveChanges();
        }
    }
}
