namespace Adria.Application.Contracts.Data;

public sealed record CosmeticItemData(
    Guid cosmeticId,
    string itemName,
    string itemType,
    decimal points
);