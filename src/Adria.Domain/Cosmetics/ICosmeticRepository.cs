namespace Adria.Domain;

public interface ICosmeticRepository
{
    Task<CosmeticItem> ById(Guid cosmeticId);
    Task Save(CosmeticItem cosmeticItem, User user);
}