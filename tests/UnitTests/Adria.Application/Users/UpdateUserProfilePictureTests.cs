using Adria.Application.Users;
using Adria.Domain;
using UnitTests.Mocks;

namespace UnitTests.Adria.Application.Users
{
    public class UpdateUserProfilePictureTests
    {
        [Fact]
        public async Task Execute_WithExistingUser_UpdatesProfilePicture()
        {
            var userId = Guid.NewGuid();
            var image = new byte[] { 1, 2, 3 };

            var user = new User(userId, "TestUser", 0m);
            var userRepository = new MockUserRepository();
            userRepository.AddUser(user);

            var logger = new MockLogger<UpdateUserProfilePicture>();
            var useCase = new UpdateUserProfilePicture(logger, userRepository);

            var input = new UpdateUserProfilePictureInput(userId, image);

            await useCase.Execute(input);

            Assert.Equal(image, user.ProfilePicture);
            Assert.Single(logger.LoggedMessages);
        }

        [Fact]
        public async Task Execute_WithNonExistingUser_ThrowsArgumentException()
        {
            var userId = Guid.NewGuid();
            var image = new byte[] { 1, 2, 3 };

            var userRepository = new MockUserRepository();
            var logger = new MockLogger<UpdateUserProfilePicture>();
            var useCase = new UpdateUserProfilePicture(logger, userRepository);

            var input = new UpdateUserProfilePictureInput(userId, image);

            var ex = await Assert.ThrowsAsync<ArgumentException>(() => useCase.Execute(input));
            Assert.Contains($"User {userId} not found.", ex.Message);
        }
    }
}