using System;
using System.Collections.Generic;

namespace Lib.CharMappers
{
    public class RussianToEnglishMapper : ICharMapper
    {
        public bool TryGetLetter(char russianLetter, out char englishLetter)
        {
            return _lowerCaseMap.TryGetValue(russianLetter, out englishLetter)
                || _upperCaseMap.TryGetValue(russianLetter, out englishLetter);
        }

        private readonly IDictionary<char, char> _lowerCaseMap = new Dictionary<char, char>()
        {
            { 'а', 'f' },
            { 'б', ',' },
            { 'в', 'd' },
            { 'г', 'u' },
            { 'д', 'l' },
            { 'е', 't' },
            { 'ё', '`' },
            { 'ж', ';' },
            { 'з', 'p' },
            { 'и', 'b' },
            { 'й', 'q' },
            { 'к', 'r' },
            { 'л', 'k' },
            { 'м', 'v' },
            { 'н', 'y' },
            { 'о', 'j' },
            { 'п', 'g' },
            { 'р', 'h' },
            { 'с', 'c' },
            { 'т', 'n' },
            { 'у', 'e' },
            { 'ф', 'a' },
            { 'х', '[' },
            { 'ц', 'w' },
            { 'ч', 'x' },
            { 'ш', 'i' },
            { 'щ', 'o' },
            { 'ъ', ']' },
            { 'ы', 's' },
            { 'ь', 'm' },
            { 'э', '\'' },
            { 'ю', '.' },
            { 'я', 'z' }
        };

        private readonly IDictionary<char, char> _upperCaseMap = new Dictionary<char, char>()
        {
            { 'А', 'F' },
            { 'Б', '<' },
            { 'В', 'D' },
            { 'Г', 'U' },
            { 'Д', 'L' },
            { 'Е', 'T' },
            { 'Ё', '~' },
            { 'Ж', ':' },
            { 'З', 'P' },
            { 'И', 'B' },
            { 'Й', 'Q' },
            { 'К', 'R' },
            { 'Л', 'K' },
            { 'М', 'V' },
            { 'Н', 'Y' },
            { 'О', 'J' },
            { 'П', 'G' },
            { 'Р', 'H' },
            { 'С', 'C' },
            { 'Т', 'N' },
            { 'У', 'E' },
            { 'Ф', 'A' },
            { 'Х', '{' },
            { 'Ц', 'W' },
            { 'Ч', 'X' },
            { 'Ш', 'I' },
            { 'Щ', 'O' },
            { 'Ъ', '}' },
            { 'Ы', 'S' },
            { 'Ь', 'M' },
            { 'Э', '"' },
            { 'Ю', '>' },
            { 'Я', 'Z' }
        };
    }
}
