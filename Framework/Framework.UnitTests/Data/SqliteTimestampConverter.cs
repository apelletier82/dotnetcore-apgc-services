
using System.Linq;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Framework.UnitTests.Data
{
    public class SqliteTimestampConverter : ValueConverter<byte[], string>
    {
        public SqliteTimestampConverter() 
            : base(
                v => v == null ? null : new string(v.Select(b => (char)b).ToArray()),
                v => v == null ? null : v.Select(c => (byte)c).ToArray()
            )
        { }
    }
}