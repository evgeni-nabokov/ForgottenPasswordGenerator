namespace Lib.CharMappers
{
    public interface ICharMapper
    {
        bool TryGetLetter(char inputLetter, out char outputLetter);
    }
}
