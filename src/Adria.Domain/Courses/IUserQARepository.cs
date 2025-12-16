namespace Adria.Domain;

public interface IUserQARepository
{
    Task<Guid> GetQAID(Guid questionId, Guid answerId);
    Task<Guid> Save(Guid userId, Guid aid, Guid qid);
    Task<int> GetProgress(Guid userId, Guid courseId);
}