namespace Adria.Application;


public sealed record CosmeticItemData(
    Guid cosmeticId,
    string itemName,
    string itemType,
    decimal points
);

