namespace Adria.Application.Contracts.Data;

public record LeaderBoardData(
    string Name,
    int PointsBalance,
    byte[]? ProfilePicture
);