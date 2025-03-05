using Repetify.Domain.Entities;

namespace Repetify.Tests.Domain.Entities;

public class UserTests
{
	[Fact]
	public void User_Should_Initialize_With_Valid_Values()
	{
		// Arrange
		var username = "JohnDoe";
		var email = "johndoe@example.com";

		// Act
		var user = new User(Guid.NewGuid(), username, email);

		// Assert
		Assert.Equal(username, user.Username);
		Assert.Equal(email, user.Email);
		Assert.NotEqual(Guid.Empty, user.Id); // Ensure ID is generated
	}

	[Fact]
	public void User_Should_Throw_Exception_When_Username_Is_Null()
	{
		// Arrange
		var email = "johndoe@example.com";

		// Act & Assert
		var exception = Assert.Throws<ArgumentNullException>(() => new User(Guid.NewGuid(), null!, email));
		Assert.Equal("Value cannot be null. (Parameter 'username')", exception.Message);
	}

	[Fact]
	public void User_Should_Throw_Exception_When_Email_Is_Null()
	{
		// Arrange
		var username = "JohnDoe";

		// Act & Assert
		var exception = Assert.Throws<ArgumentNullException>(() => new User(Guid.NewGuid(), username, null!));
		Assert.Equal("Value cannot be null. (Parameter 'email')", exception.Message);
	}
}
