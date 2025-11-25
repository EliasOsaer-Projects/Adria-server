namespace Taskly.Infrastructure.Persistence.Shared;

public sealed class AdriaDatabaseException(string message, Exception? innerException = null)
    : Exception(message, innerException)
{
    public AdriaDatabaseException()
        : this("An error occurred while accessing the Adria database.")
    {
    }

    public AdriaDatabaseException(Exception innerException)
        : this("An error occurred while accessing the Adria database.", innerException)
    {
    }
}