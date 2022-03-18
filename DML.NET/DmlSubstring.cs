namespace ToolBX.DML.NET;

public record DmlSubstring
{
    public string Text { get; init; } = string.Empty;
    public Color? Color { get; init; }
    public int Length => Text.Length;
    public IReadOnlyList<TextStyle> Styles { get; init; } = Array.Empty<TextStyle>();

    public override string ToString()
    {
        if (string.IsNullOrWhiteSpace(Text)) return "(Empty)";
        return Color == null ? Text : $"'{Text}' colored {Color}";
    }
}