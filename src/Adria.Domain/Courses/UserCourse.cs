namespace Adria.Domain;

public class UserCourse
{
    public Guid AdriaId { get; set; }
    public Guid CourseId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int ProgressPercentage { get; set; }
    public Boolean CompletionStatus { get; set; }

    public UserCourse(Guid adriaId, Guid courseId)
    {
        AdriaId = adriaId;
        CourseId = courseId;
        StartDate = DateTime.Now;
        ProgressPercentage = 0;
        CompletionStatus = false;
    }

    public void UpdateProgress(int progressPercentage)
    {
        ProgressPercentage += progressPercentage;
        if (ProgressPercentage >= 100)
        {
            CompletionStatus = true;
            ProgressPercentage = 100;
            EndDate = DateTime.Now;
        }
    }
}
