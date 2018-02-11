using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
