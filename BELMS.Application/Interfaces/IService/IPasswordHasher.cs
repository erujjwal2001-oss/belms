namespace BELMS.Application.Interfaces.IService;

public interface IPasswordHasher
{
    string Hash(string password);

    bool Verify(string password, string passwordHash);
}
