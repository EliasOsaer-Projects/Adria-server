using Adria.Domain;


namespace UnitTests.Adria.Domain;


public sealed class CourseTests
{
    [Fact]
    public void Constructor_WithGuid_SetsAllProperties()
    {
        // Arrange
        var id = Guid.NewGuid();
        var name = "C# Basics";
        var category = "Development";
        var description = "Learn the basics of C#.";
        var learnList = new List<string> { "variables", "loops", "classes" };
        var duration = 8;
        var levelInt = 1; 
        var points = 100;
        var tags = new List<string>{"fun", "coding"};

        // Act
        var course = new Course(id, name, levelInt, description, category, tags, points, learnList, duration, new Guid(), new Guid(), false);

        // Assert
        Assert.Equal(id, course.Id);
        Assert.Equal(name, course.Name);
        Assert.Equal(category, course.Category);
        Assert.Equal(description, course.Description);
        Assert.Equal(tags, course.Tags);
        Assert.Same(learnList, course.LearnList); 
        Assert.Equal(duration, course.Duration);
        Assert.Equal(1, course.Level);
        Assert.Equal(points, course.PointsOnCompletion);
    }
  
    [Fact]
    public void Constructor_WithoutGuid_GeneratesNonEmptyId()
    {
        // Arrange
        var name = "Advanced LINQ";
        var category = "Development";
        var description = "Deep dive into LINQ.";
        var learnList = new List<string> { "query syntax", "method syntax" };
        var duration = 6;
        var levelInt = 2;
        var points = 50;
        var tags = new List<string>{"fun", "coding"};

        // Act
        var course = new Course(name, levelInt, description, category, tags, points, learnList, duration, new Guid(), new Guid(), false);

        // Assert
        Assert.NotEqual(Guid.Empty, course.Id);
        Assert.Equal(name, course.Name);
        Assert.Equal(2, course.Level);
    }
    

    [Fact]
    public void Level_IsProperlyCastFromInt()
    {
        // Arrange
        var name = "Some course";
        var category = "Cat";
        var description = "Desc";
        var learnList = new List<string>();
        var duration = 1;
        var levelInt = 2; 
        var modules = new List<Module>();
        var points = 0;
        var tags = new List<string>{"fun", "coding"};

        // Act
        var course = new Course(name, levelInt, description, category, tags, points, learnList, duration, new Guid(), new Guid(), false);

        // Assert
        Assert.IsType<int>(course.Level);
        Assert.Equal(2, course.Level);
    }
}  