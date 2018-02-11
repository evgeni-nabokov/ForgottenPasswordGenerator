using System.Text;

namespace Lib.PasswordSections
{
    public interface IPasswordSection
    {
        int MaxLength { get; }
        int MinLength { get; }
        StringBuilder CurrentCombination { get; }
        ulong GetCombinationCount();
        bool MoveToNextState();
        void ResetState();
    }
}
