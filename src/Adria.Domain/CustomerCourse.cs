namespace Adria.Domain;

public class CustomerCourse
{
    public Guid Id { get; private set; }

    public Course Course { get; }

    public Boolean CompletionStatus { get; private set; }

    public Decimal Progress { get; private set; }


    public CustomerCourse(Guid customerCourseId, Course course)
    {
        Id = Guid.NewGuid();
        Course = course;
        Progress = 0;
        CompletionStatus = false;
    }
}