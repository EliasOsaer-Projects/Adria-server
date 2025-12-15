using System.Data.Common;
using Adria.Domain;
using Adria.Infrastructure.Persistence.Shared;
using Microsoft.Extensions.Logging;
using Taskly.Infrastructure.Persistence.Shared;

namespace Adria.Infrastructure.Persistence.Repositories;

public class AdoUserQARepository
    :AbstractAdoRepository, IUserQARepository
{
    private readonly ILogger<AdoUserQARepository> _logger;
    
    private static readonly string SELECT_QAID = @"
        SELECT QAID
        FROM QuestionAnswer
        WHERE QuestionID = @QuestionID AND AnswerID = @AnswerID;
    ";
    
    private static readonly string INSERT_USER_QA = @"
        INSERT INTO UserQuestionAnswer (QAID, AdriaId, IsCorrectlyAnswered)
        VALUES (@QAID, @UserID, @CorrectlyAnswered);
    ";
    
    private static readonly string SELECT_CORRECT_ANSWER_ID = @"
        SELECT a.AnswerID
        FROM QuestionAnswer qa
        INNER JOIN Answer a ON qa.AnswerID = a.AnswerID
        WHERE qa.QuestionID = @QuestionID AND a.IsCorrect = 1
        LIMIT 1;
    ";
    
    public AdoUserQARepository(
        DbProviderFactory factory, 
        string connectionString,
        ILogger<AdoUserQARepository> logger
    ) : base(factory, connectionString)
    {
        _logger = logger;   
    }

    public async Task<Guid> GetQAID(Guid questionId, Guid answerId)
    {
        DbParameter qid = CreateParameter("@QuestionID", questionId);
        DbParameter aid = CreateParameter("@AnswerID", answerId);
        DbDataReader dbDataReader = await ExecuteReaderAsync(SELECT_QAID, [qid, aid]);
        try
        {
            if (await dbDataReader.ReadAsync())
            {
                return dbDataReader.GetGuid(dbDataReader.GetOrdinal("QAID"));
            }
        }
        catch (DbException e)
        {
            _logger.LogError(e, "Failed to read QAID");
            throw new AdriaDatabaseException("Failed to read QAID from database.", e);
        }
        finally
        {
            _logger.LogInformation("Disposing DbDataReader");
            await dbDataReader.DisposeAsync();
        }
        
        return Guid.Empty;
    }

    public async Task<Guid> GetCorrectAnswerId(Guid questionId)
    {
        DbParameter qid = CreateParameter("@QuestionID", questionId);
        DbDataReader dbDataReader = await ExecuteReaderAsync(SELECT_CORRECT_ANSWER_ID, [qid]);
        try
        {
            if (await dbDataReader.ReadAsync())
            {
                return dbDataReader.GetGuid(dbDataReader.GetOrdinal("AnswerID"));
            }
        }
        catch (DbException e)
        {
            _logger.LogError(e, "Failed to read Correct Answer ID");
            throw new AdriaDatabaseException("Failed to read Correct Answer ID from database.", e);
        }
        finally
        {
            _logger.LogInformation("Disposing DbDataReader");
            await dbDataReader.DisposeAsync();
        }
        
        return Guid.Empty;
    }
    
    public async Task<Guid> Save(Guid userId, Guid aid, Guid qid)
    {
        DbParameter userIdParam = CreateParameter("@UserID", userId);
        DbParameter answerIdParam = CreateParameter("@AnswerID", aid);
        DbParameter questionIdParam = CreateParameter("@QuestionID", qid);

        var qaid = await GetQAID(qid, aid);
        bool correctlyAnswered = await GetCorrectAnswerId(qid) == aid;
        
        DbParameter qaidParam = CreateParameter("@QAID", qaid);
        DbParameter correctParam = CreateParameter("@CorrectlyAnswered", correctlyAnswered);
        
        await ExecuteNonQueryAsync(INSERT_USER_QA, [qaidParam, userIdParam, correctParam]);
        return await GetCorrectAnswerId(qid);
    }
}