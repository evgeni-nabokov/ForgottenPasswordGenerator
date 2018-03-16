using Lib;

namespace Cli.Params
{
    public abstract class SectionParamsBase
    {
        public string Chars { get; set; }
        public CharCase CharCase { get; set; }
        public int? MinLength { get; set; }
    }
}
