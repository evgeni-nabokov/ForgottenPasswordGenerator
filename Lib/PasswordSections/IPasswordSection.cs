using System.Collections.Generic;

namespace Lib.PasswordSections
{
    public interface IPasswordSection : IEnumerator<string>
    {
        int MaxLength { get; }
        int MinLength { get; }
        ulong Count { get; }
        IEnumerable<string> GetVariations();
    }
}
