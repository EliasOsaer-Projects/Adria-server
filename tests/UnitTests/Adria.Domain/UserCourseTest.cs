using Adria.Domain;

namespace UnitTests.Adria.Domain;

public class UserCourseTest
{
    [Fact]
    public void Constructor_WithValidParameters_SetsAllProperties()
    {
        // Arrange
        var adriaId = Guid.NewGuid();
        var courseId = Guid.NewGuid();
        var startDate = DateTime.Now.Date;
        var progress = 0;
        var completionStatus = false;
        
        // Act
        var userCourse = new UserCourse(
            adriaId,
            courseId
        );
        
        // Assert
        Assert.Equal(adriaId, userCourse.AdriaId);
        Assert.Equal(courseId, userCourse.CourseId);
        Assert.Equal(startDate, userCourse.StartDate.Date);
        Assert.Equal(progress, userCourse.ProgressPercentage);
        Assert.Equal(completionStatus, userCourse.CompletionStatus);
    }
    
    [Fact]
    public void UpdateProgress_UpdatesProgress()
    {
        // Arrange
        var userCourse = new UserCourse(
            Guid.NewGuid(),
            Guid.NewGuid()
        );
        
        // Act
        userCourse.UpdateProgress(50);
        
        // Assert
        Assert.Equal(50, userCourse.ProgressPercentage);
    }
    
    [Fact]
    public void UpdateProgess_SetsCompletionStatus()
    {
        // Arrange
        var userCourse = new UserCourse(
            Guid.NewGuid(),
            Guid.NewGuid()
        );
        
        // Act
        userCourse.UpdateProgress(100);
        
        // Assert
        Assert.True(userCourse.CompletionStatus);
        Assert.Equal(100, userCourse.ProgressPercentage);
        Assert.Equal(DateTime.Now.Date, userCourse.EndDate.Date);
    }
}