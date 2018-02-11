namespace Lib.LetterMappers
{
    public interface ILetterMapper
    {
        bool TryGetLetter(char inputLetter, out char outputLetter);
    }
}
