namespace Cli.Params
{
    internal class PatternParams
    {
        public string OutputFilename { get; set; }
        public int? MaxSingeCharSequence { get; set; }
        public SectionParamsBase[] Sections { get; set; }
    }
}
