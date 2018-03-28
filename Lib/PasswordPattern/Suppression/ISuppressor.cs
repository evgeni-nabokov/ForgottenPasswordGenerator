using System.Text;

namespace Lib.PasswordPattern.Suppression
{
    public interface ISuppressor
    {
        bool BreaksRestrictions(StringBuilder variation);
    }
}
