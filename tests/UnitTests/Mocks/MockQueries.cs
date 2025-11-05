using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;

namespace UnitTests.Mocks;

public class MockUserByIdQuery : IUserByIdQuery
{
    private readonly Dictionary<Guid, UserData> _users = new();

    public Task<UserData?> Fetch(Guid userId)
    {
        _users.TryGetValue(userId, out UserData? user);
        return Task.FromResult(user);
    }

    public void AddUser(UserData user)
    {
        _users[user.Id] = user;
    }

    public void Clear()
    {
        _users.Clear();
    }
}