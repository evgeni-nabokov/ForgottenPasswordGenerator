namespace Cli.Params
{
    internal class NumberRangeSectionParams : SectionParamsBase
    {
        public int MinValue { get; set; }
        public int MaxValue { get; set; }
        public int? Step { get; set; }
    }
}
