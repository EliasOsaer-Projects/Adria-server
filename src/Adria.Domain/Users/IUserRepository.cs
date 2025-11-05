namespace Adria.Domain;

public interface IUserRepository
{
    Task<User?> ById(Guid userId);
    
    Task Save(User user);
}