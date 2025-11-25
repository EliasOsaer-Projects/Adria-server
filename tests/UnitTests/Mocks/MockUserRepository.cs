using Adria.Domain;
using Adria.Domain.Users;

namespace UnitTests.Mocks;

public class MockUserRepository : IUserRepository
{
    private readonly Dictionary<Guid, User> _users = new();

    public Task<User?> ById(Guid adriaId)
    {
        _users.TryGetValue(adriaId, out User? user);
        return Task.FromResult(user);
    }

    public Task Save(User user)
    {
        _users[user.AdriaId] = user;
        return Task.CompletedTask;
    }

    public void AddUser(User user)
    {
        _users[user.AdriaId] = user;
    }

    public void Clear()
    {
        _users.Clear();
    }
}