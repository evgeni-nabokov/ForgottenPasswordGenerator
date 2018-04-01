namespace Lib.Suppressors
{
    public interface ISuppressor
    {
        bool BreaksRestrictions(string variation);
    }
}
