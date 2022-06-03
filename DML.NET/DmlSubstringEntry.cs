namespace ToolBX.DML.NET;

public record DmlSubstringEntry
{
    private readonly DmlSubstring _substring;

    public string Text => _substring.Text;
    public Color? Color => _substring.Color;
    public Color? Highlight => _substring.Highlight;
    public int Length => _substring.Length;
    public IReadOnlyList<TextStyle> Styles => _substring.Styles;

    public int StartIndex { get; init; }
    public int EndIndex => StartIndex + _substring.Length;

    //TODO Internal?
    public DmlSubstringEntry(DmlSubstring substring)
    {
        _substring = substring ?? throw new ArgumentNullException(nameof(substring));
    }

    public static implicit operator DmlSubstring(DmlSubstringEntry entry) => entry._substring with { };

    public override string ToString() => _substring.ToString();
}