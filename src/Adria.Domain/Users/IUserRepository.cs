namespace Adria.Domain.Users;

public interface IUserRepository
{
    Task<User?> GetById(Guid adriaId);
    Task Save(User user);
}