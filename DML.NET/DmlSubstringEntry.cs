namespace ToolBX.DML.NET;

public sealed record DmlSubstringEntry
{
    private readonly DmlSubstring _substring;

    public string Text => _substring.Text;
    public Color? Color => _substring.Color;
    public string? ColorName => _substring.ColorName;
    public Color? Highlight => _substring.Highlight;
    public string? HighlightName => _substring.HighlightName;
    public string? Keyword => _substring.Keyword;
    public bool IsProfanity => _substring.IsProfanity;
    public ProfanityLevel? ProfanityLevel => _substring.ProfanityLevel;
    public string? Clean => _substring.Clean;
    public int Length => _substring.Length;
    public IReadOnlyList<TextStyle> Styles => _substring.Styles;

    public int StartIndex { get; init; }
    public int EndIndex => StartIndex + _substring.Length;

    //TODO Internal?
    public DmlSubstringEntry(DmlSubstring substring)
    {
        ArgumentNullException.ThrowIfNull(substring);
        _substring = substring;
    }

    public static implicit operator DmlSubstring(DmlSubstringEntry entry) => entry._substring with { };

    public override string ToString() => _substring.ToString();
}