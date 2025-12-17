namespace Adria.Application.Contracts.Data;

public record LeaderBoardData(
    Guid UserId,
    string Name,
    int PointsBalance,
    byte[]? ProfilePicture
);