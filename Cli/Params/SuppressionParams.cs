namespace Cli.Params
{
    public class SuppressionParams
    {
        public string ForbiddenDuplicateChars { get; set; }
        public int? AdjacentDuplicateMaxLength { get; set; }
        public int? CapitalAdjacentMaxLength { get; set; }
        public int? CapitalCharMinDistance { get; set; }
        public string Regex { get; set; }
    }
}
