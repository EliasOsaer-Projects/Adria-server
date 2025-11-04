namespace Adria.Domain;

public class CustomerCourse
{
    public Guid CustomerCourseId { get; private set; }

    public Course Course { get; }

    public Boolean CompletionStatus { get; private set; }

    public Decimal Progress { get; private set; }


    public CustomerCourse(Guid customerCourseId, Course course)
    {
        CustomerCourseId = customerCourseId;
        Course = course;
        Progress = 0;
        CompletionStatus = false;
    }
}