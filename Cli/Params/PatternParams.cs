namespace Cli.Params
{
    internal class PatternParams
    {
        public int? MaxSingeCharSequenceLength { get; set; }
        public int? MaxCapitalLetterSequenceLength { get; set; }
        public int? MinCapitalCharDistance { get; set; }
        public string CharMapper { get; set; }
        public SectionParamsBase[] Sections { get; set; }
    }
}
