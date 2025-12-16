using Adria.Application.Users;
using Adria.Domain;
using UnitTests.Mocks;
using Microsoft.Extensions.Logging.Abstractions;

namespace UnitTests.Adria.Application.Users
{
    public class DeleteUserProfilePictureTests
    {
        [Fact]
        public async Task Execute_WithExistingUser_SetsProfilePictureToNull()
        {
            var userId = Guid.NewGuid();
            var user = new User(userId, "TestUser", 150m) { ProfilePicture = new byte[] { 1, 2, 3 } };
            var mockRepo = new MockUserRepository();
            var mocklogger = new MockLogger<DeleteUserProfilePicture>();
            await mockRepo.Save(user);

            var useCase = new DeleteUserProfilePicture(mocklogger, mockRepo);

            await useCase.Execute(userId);

            var updatedUser = await mockRepo.ById(userId);
            Assert.NotNull(updatedUser);
            Assert.Null(updatedUser.ProfilePicture);
        }

        [Fact]
        public async Task Execute_WithNonExistentUser_ThrowsArgumentException()
        {
            var userId = Guid.NewGuid();
            var mockRepo = new MockUserRepository();

            var useCase = new DeleteUserProfilePicture(NullLogger<DeleteUserProfilePicture>.Instance, mockRepo);

            var ex = await Assert.ThrowsAsync<ArgumentException>(() => useCase.Execute(userId));
            Assert.Contains($"User {userId} not found", ex.Message);
        }
    }
}