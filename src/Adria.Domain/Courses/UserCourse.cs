namespace Adria.Domain;

public class UserCourse
{
    public Guid AdriaId { get; set; }
    public Guid CourseId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
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

    public UserCourse(Guid adriaId, Guid courseId, DateTime startDate, DateTime? endDate, int progress, bool complete)
    {
        EnsureCompletionIsValid(complete, progress);

        AdriaId = adriaId;
        CourseId = courseId;
        StartDate = startDate;
        EndDate = endDate;
        ProgressPercentage = progress;
        CompletionStatus = complete;
    }

    public void UpdateProgress(int progressPercentage)
    {
        ProgressPercentage = progressPercentage;
        if (ProgressPercentage >= 100)
        {
            CompletionStatus = true;
            ProgressPercentage = 100;
            EndDate = DateTime.Now;
        }
    }

    private static void EnsureCompletionIsValid(bool complete, int progress)
    {
        if (complete && progress < 100 || !complete && progress == 100)
            throw new GenesisException("The progress and completion status do not match");
    }
}
