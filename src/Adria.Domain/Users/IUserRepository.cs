namespace Adria.Domain.Users;

public interface IUserRepository
{
    Task<User?> ById(Guid adriaId);
    Task Save(User user);
}