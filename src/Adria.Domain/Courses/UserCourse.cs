namespace Adria.Domain;

public class UserCourse
{
    public Guid AdriaId { get; set; }
    public Guid CourseId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int ProgressPercentage { get; set; }
    public Boolean CompletionStatus { get; set; }

    public UserCourse(Guid adriaId, Guid courseId, DateTime startDate, int progressPercentage, bool completionStatus)
    {
        AdriaId = adriaId;
        CourseId = courseId;
        StartDate = DateTime.Now;
        ProgressPercentage = 0;
        CompletionStatus = false;
    }
}