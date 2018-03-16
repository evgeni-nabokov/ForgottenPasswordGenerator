using Lib;

namespace Cli.Params
{
    public class ArbitrarySectionParams : SectionParamsBase
    {
        public string Chars { get; set; }
        public int? MinLength { get; set; }
        public int MaxLength { get; set; }
        public CharCase CharCase { get; set; }
    }
}
