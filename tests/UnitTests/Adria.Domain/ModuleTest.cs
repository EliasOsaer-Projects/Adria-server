using Adria.Domain;

namespace UnitTests.Adria.Domain;

public class ModuleTest
{
    [Fact]
    public void Constructor_WithValidParameters_SetsAllProperties()
    {
        // Arrange
        var id = Guid.NewGuid();
        var courseId = Guid.NewGuid();
        var position = 1;
        var content = "Learn how to use the Console class.";
        
        // Act
        var module = new Module(
            id,
            courseId,
            position,
            content
        );
        
        // Assert
        Assert.Equal(id, module.Id);
        Assert.Equal(courseId, module.CourseId);
        Assert.Equal(position, module.Position);
        Assert.Equal(content, module.Content);
    }
    
    [Fact]
    public void Constructor_WithInvalidParameters_NoIdSet()
    {
        // Arrange
        var courseId = Guid.NewGuid();
        var position = 0;
        var content = "";
        
        // Act & Assert
        var module = new Module(courseId, position, content);
        
        // Assert
        Assert.Equal(typeof(Guid), module.Id.GetType());
    }
}