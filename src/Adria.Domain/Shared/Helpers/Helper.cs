using System.Data.Common;

namespace Adria.Domain.Shared.Helpers;

public class Helper
{
    public static async Task<byte[]> ReadBlobAsync(DbDataReader reader, int ordinal)
    {
        if (reader.IsDBNull(ordinal))
            return Array.Empty<byte>();

        const int bufferSize = 8192;
        using var ms = new MemoryStream();
        long bytesRead = 0;
        long fieldOffset = 0;
        byte[] buffer = new byte[bufferSize];

        // Get length of blob
        long blobLength = reader.GetBytes(ordinal, 0, null, 0, 0);
        while (bytesRead < blobLength)
        {
            int read = (int)Math.Min(bufferSize, blobLength - bytesRead);
            int actualRead = (int)reader.GetBytes(ordinal, fieldOffset, buffer, 0, read);
            ms.Write(buffer, 0, actualRead);
            bytesRead += actualRead;
            fieldOffset += actualRead;
        }
        return ms.ToArray();
    }
}