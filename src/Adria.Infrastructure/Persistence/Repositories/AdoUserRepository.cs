using System.Data.Common;
using Adria.Domain;
using Adria.Domain.Shared.Helpers;
using Adria.Domain.Users;
using Adria.Infrastructure.Persistence.Shared;
using Microsoft.Extensions.Logging;
using Taskly.Infrastructure.Persistence.Shared;

namespace Adria.Infrastructure.Persistence.Repositories;

public sealed class AdoUserRepository : AbstractAdoRepository, IUserRepository
{
    private readonly ILogger<AdoUserRepository> _logger;
    private static readonly string TABLE_USERS = "User";
    private static readonly string COL_ID = "AdriaID";
    private static readonly string COL_NAME = "Name";
    private static readonly string COL_WALLET = "Wallet";
    private static readonly string COL_POINTS = "PointsBalance";
    private static readonly string COL_STREAK = "Streak";
    private static readonly string COL_OCCUPATION = "Occupation";
    private static readonly string COL_PROFILE_PICTURE = "ProfilePicture";

    private static readonly string SELECT_USER_BY_ID = $@"
        SELECT {COL_ID}, {COL_NAME}, {COL_WALLET}, {COL_POINTS}, {COL_STREAK}, {COL_OCCUPATION}, {COL_PROFILE_PICTURE}
        FROM {TABLE_USERS} 
        WHERE {COL_ID} = @UserId;
    ";
    
    private static readonly string INSERT_USER = $@"
        INSERT INTO {TABLE_USERS} ({COL_ID}, {COL_NAME}, {COL_WALLET}, {COL_POINTS}, {COL_STREAK})
        VALUES (@Id, @Name, @Wallet, @Points, @Streak);
    ";
    
    private static readonly string UPDATE_USER = $@"
        UPDATE {TABLE_USERS}
        SET {COL_NAME} = @Name, {COL_WALLET} = @Wallet, {COL_POINTS} = @Points, {COL_STREAK} = @Streak, {COL_PROFILE_PICTURE} = @ProfilePicture
        WHERE {COL_ID} = @Id;
    ";

    public AdoUserRepository(
        DbProviderFactory factory,
        string connectionString,
        ILogger<AdoUserRepository> logger
    ) : base(factory, connectionString)
    {
        _logger = logger;
    }

    public async Task<User?> ById(Guid adriaId)
    {
        DbParameter id = CreateParameter("@UserId", adriaId.ToString());
        DbDataReader dbDataReader = await ExecuteReaderAsync(SELECT_USER_BY_ID, [id]);

        try
        {
            if (await dbDataReader.ReadAsync())
            {
                byte[]? profilePicture = await dbDataReader.IsDBNullAsync(dbDataReader.GetOrdinal("ProfilePicture")) 
                    ? null
                    : await Helper.ReadBlobAsync(dbDataReader, dbDataReader.GetOrdinal("ProfilePicture"));
                return new User(
                    dbDataReader.GetGuid(dbDataReader.GetOrdinal(COL_ID)),
                    dbDataReader.GetString(dbDataReader.GetOrdinal(COL_NAME)),
                    dbDataReader.GetDecimal(dbDataReader.GetOrdinal(COL_WALLET)),
                    dbDataReader.GetInt32(dbDataReader.GetOrdinal(COL_POINTS)),
                    dbDataReader.GetInt32(dbDataReader.GetOrdinal(COL_STREAK)),
                    dbDataReader.IsDBNull(dbDataReader.GetOrdinal(COL_OCCUPATION)) ? "" : dbDataReader.GetString(dbDataReader.GetOrdinal(COL_OCCUPATION)),
                    profilePicture
                );
            }

            return null;
        }
        catch (DbException ex)
        {
            _logger.LogError(ex, "Failed to read user with ID {UserId} from database.", adriaId);
            throw new AdriaDatabaseException("Failed to read user from database.", ex);
        }
        finally
        {
            _logger.LogInformation("Disposing DbDataReader for user with ID {UserId}.", adriaId);
            await dbDataReader.DisposeAsync();
        }
    }

    public async Task Save(User user)
    {
        _logger.LogInformation("Saving user with ID {UserId} to database.", user.AdriaId);
        string userQuery = INSERT_USER;

        if ((await ById(user.AdriaId)) != null)
        {
            userQuery = UPDATE_USER;
        }

        try
        {
            DbParameter[] parameters =
            [
                CreateParameter("@Id", user.AdriaId.ToString()),
                CreateParameter("@Name", user.Name),
                CreateParameter("@Wallet", user.Wallet),
                CreateParameter("@Points", user.PointsBalance),
                CreateParameter("@Streak", user.Streak),
                user.ProfilePicture != null
                    ? CreateParameter("@ProfilePicture", user.ProfilePicture)
                    : CreateParameter("@ProfilePicture", DBNull.Value)
            ];

            await ExecuteNonQueryAsync(userQuery, parameters);

        }
        catch (DbException ex)
        {
            _logger.LogError(ex, "Failed to save user with ID {UserId} to database.", user.AdriaId);
            throw new AdriaDatabaseException("Failed to save user to database.", ex);
        }
    }
}