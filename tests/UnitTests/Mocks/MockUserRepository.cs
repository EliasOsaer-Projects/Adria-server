using Adria.Domain;

namespace UnitTests.Mocks;

public class MockUserRepository
{
    private readonly Dictionary<Guid, User> _users = new();

    public Task<User?> ById(Guid userId)
    {
        _users.TryGetValue(userId, out User? user);
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