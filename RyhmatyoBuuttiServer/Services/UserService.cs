using System;

namespace RyhmatyoBuuttiServer.Services
{
    public interface IUserService
    {
        string GenerateAccessCode(int codeLength);
    }
    public class UserService : IUserService
    {
        public string GenerateAccessCode(int codeLength)
        {
            string code = "";
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

            for (int i = 0; i < codeLength; i++)
            {
                code += chars[new Random().Next(chars.Length)];
            }

            return code;
        }
    }
}
