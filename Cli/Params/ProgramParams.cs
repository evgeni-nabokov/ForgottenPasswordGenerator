using Cli.Params.VariationGenerators;

namespace Cli.Params
{
    internal class ProgramParams : CompoundVariationGeneratorParams
    {
        public OutputStream Output { get; set; } = OutputStream.File;

        public MongoDBParams MongoDB { get; set; }
    }

    internal class MongoDBParams
    {
        public string ConnectionString { get; set; }

        public string Database { get; set; }

        public string Collection { get; set; }

        public string User { get; set; }

        public string Password { get; set; }
    }

    public enum OutputStream
    {
        File,
        Stdout,
        MongoDB
    }
}
