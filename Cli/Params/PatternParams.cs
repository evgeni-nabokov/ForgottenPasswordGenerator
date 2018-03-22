namespace Cli.Params
{
    internal class PatternParams
    {
        public int? MaxSingeCharSequenceLength { get; set; }
        public int? MaxCapitalLetterSequenceLength { get; set; }
        public int? MinCapitalCharDistance { get; set; }
        public OutputStream Output { get; set; } = OutputStream.File;
        public string CharMapper { get; set; }
        public SectionParamsBase[] Sections { get; set; }
    }

    public enum OutputStream
    {
        File,
        Stdout
    }
}
