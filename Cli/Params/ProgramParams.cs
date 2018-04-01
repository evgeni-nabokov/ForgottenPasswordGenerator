using Cli.Params.VariationGenerators;

namespace Cli.Params
{
    internal class ProgramParams : CompoundVariationGeneratorParams
    {
        public OutputStream Output { get; set; } = OutputStream.File;
    }

    public enum OutputStream
    {
        File,
        Stdout
    }
}
