namespace Adria.Domain;

public class CustomerCourse
{
    public Guid Id { get; private set; }

    public Course Course { get; }

    public Boolean CompletionStatus { get; private set; }

    public Decimal Progress { get; private set; }


    public CustomerCourse(Guid id, Course course)
    {
        Id = id;
        Course = course;
        Progress = 0;
        CompletionStatus = false;
    }
    
    public CustomerCourse(Course course) :  this(Guid.NewGuid(), course)
    {
     
    }
    
    
    public CustomerCourse(Guid id, Course course, Decimal progress, Boolean completionStatus)
    {
        Id = id;
        Course = course;
        Progress = progress;
        CompletionStatus = completionStatus;
    }
}