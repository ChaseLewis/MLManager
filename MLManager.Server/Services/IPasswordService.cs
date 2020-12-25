namespace MLManager.Services
{
    public interface IPasswordService
    {
        string HashPassword(string password);
        bool CheckPassword(string password,string passwordHash);
    }
}