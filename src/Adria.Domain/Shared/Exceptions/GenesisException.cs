namespace Adria.Domain;

public class GenesisException : ArgumentException
{
    public GenesisException(string message) : base(message)
    {
    }
}
