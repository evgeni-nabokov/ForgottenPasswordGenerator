using System.Text;

namespace Lib.PasswordSections
{
    public interface IPasswordSection
    {
        int MaxLength { get; }
        int MinLength { get; }
        StringBuilder GetCurrentCombination();
        ulong GetCombinationCount();
        bool MoveToNextState();
        void ResetState();
    }
}
