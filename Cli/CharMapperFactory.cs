using Lib.CharMappers;

namespace Cli
{
    public static class CharMapperFactory
    {
        public static ICharMapper CreateCharMapper(string charMapName)
        {
            switch (charMapName)
            {
                case "RussianToEnglish":
                    return new RussianToEnglishMapper();
                default:
                    return null;
            }
        }
    }
}
