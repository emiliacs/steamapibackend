using RyhmatyoBuuttiServer.Models;

namespace RyhmatyoBuuttiServer.Repositories
{
    public interface IResetCodeRepository
    {
        PasswordResetCode findResetCodeByUserId(long id);
        void AddResetCode(PasswordResetCode prc);
        void UpdateResetCode(PasswordResetCode prc);

    }
}
