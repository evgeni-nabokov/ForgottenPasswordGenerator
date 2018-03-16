using Lib;

namespace Cli.Params
{
    internal class FixedSectionParams : SectionParamsBase
    {
        public string Chars { get; set; }
        public CharCase CharCase { get; set; }
        public int? MinLength { get; set; }
    }
}
