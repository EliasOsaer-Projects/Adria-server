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
    
    private static readonly string sql_calculateProgress = @"
    SELECT 
        (SELECT COUNT(DISTINCT qa.QuestionID)
         FROM QuestionAnswer qa
         INNER JOIN Module m ON qa.ModuleID = m.ModuleID
         WHERE m.CourseID = @courseId) as TotalQuestions,
        
        (SELECT COUNT(DISTINCT qa.QuestionID)
         FROM QuestionAnswer qa
         INNER JOIN Module m ON qa.ModuleID = m.ModuleID
         INNER JOIN UserQuestionAnswer uqa ON qa.QAID = uqa.QAID
         WHERE m.CourseID = @courseId
           AND uqa.AdriaID = @adriaId
           AND uqa.IsCorrectlyAnswered = TRUE) as CorrectQuestions;
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
    
    public async Task<int> GetProgress(Guid userId, Guid courseId)
    {
        DbParameter adriaIdParam = CreateParameter("@adriaId", userId);
        DbParameter courseIdParam = CreateParameter("@courseId", courseId);
    
        DbDataReader dbDataReader = await ExecuteReaderAsync(sql_calculateProgress, [adriaIdParam, courseIdParam]);
        try
        {
            if (await dbDataReader.ReadAsync())
            {
                int totalQuestions = dbDataReader.GetInt32(0);
                int correctQuestions = dbDataReader.GetInt32(1);
            
                if (totalQuestions == 0) return 0;
            
                var progress = (int)Math.Round((double)correctQuestions / totalQuestions * 100);
                
                return progress;
            }
        }
        catch (DbException e)
        {
            _logger.LogError(e, "Failed to calculate progress");
            throw new AdriaDatabaseException("Failed to calculate progress from database.", e);
        }
        finally
        {
            await dbDataReader.DisposeAsync();
        }

        return 0;
    }

    
    public async Task<Guid> Save(Guid userId, Guid aid, Guid qid)
    {
        DbParameter userIdParam = CreateParameter("@UserID", userId);

        var qaid = await GetQAID(qid, aid);
        bool correctlyAnswered = await GetCorrectAnswerId(qid) == aid;
        
        DbParameter qaidParam = CreateParameter("@QAID", qaid);
        DbParameter correctParam = CreateParameter("@CorrectlyAnswered", correctlyAnswered);
        
        await ExecuteNonQueryAsync(INSERT_USER_QA, [qaidParam, userIdParam, correctParam]);
        return await GetCorrectAnswerId(qid);
    }
}