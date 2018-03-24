namespace Lib.PatternParser
{
    internal struct PatternPiece
    {
        public PatternPiece(string content, int startIndex, PatternPieceType type)
        {
            Content = content;
            StartIndex = startIndex;
            Type = type;
        }

        public string Content { get; }
        public int StartIndex { get; }
        public PatternPieceType Type { get; }
    }

    internal enum PatternPieceType
    {
        PlainString,
        BraceContent
    }
}
