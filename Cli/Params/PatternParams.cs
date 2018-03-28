namespace Cli.Params
{
    internal class PatternParams
    {
        public SuppressionParams Suppression { get; set; }
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
