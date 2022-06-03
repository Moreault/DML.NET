namespace ToolBX.DML.NET;

public record DmlSubstring
{
    /// <summary>
    /// Clean text without any DML tags.
    /// </summary>
    public string Text { get; init; } = string.Empty;

    /// <summary>
    /// Color that the text should be written in.
    /// </summary>
    public Color? Color { get; init; }

    /// <summary>
    /// Color that the background behind the text should be highlighted in.
    /// </summary>
    public Color? Highlight { get; init; }

    /// <summary>
    /// Number of characters in the string (excluding DML tags.)
    /// </summary>
    public int Length => Text.Length;

    /// <summary>
    /// All the text styles contained in the substring (ex: Bold, Italic, Strikeout, etc...)
    /// </summary>
    public IReadOnlyList<TextStyle> Styles { get; init; } = Array.Empty<TextStyle>();

    public override string ToString()
    {
        if (string.IsNullOrWhiteSpace(Text)) return "(Empty)";
        return Color == null ? Text : $"'{Text}' colored {Color}";
    }
}