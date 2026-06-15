namespace ToolBX.DML.NET;

public sealed record DmlSubstring
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
    /// Identifier of the keyword this text refers to, if any. DML does not interpret this value: it is up to the
    /// consuming application to resolve it (ex: to color the text, make it clickable, fetch a tooltip, etc.) When a
    /// keyword tag is present without an explicit id, this defaults to <see cref="Text"/>.
    /// </summary>
    public string? Keyword { get; init; }

    /// <summary>
    /// Number of characters in the string (excluding DML tags.)
    /// </summary>
    public int Length => Text.Length;

    /// <summary>
    /// All the text styles contained in the substring (ex: Bold, Italic, Strikeout, etc...)
    /// </summary>
    public IReadOnlyList<TextStyle> Styles { get; init; } = [];

    public override string ToString()
    {
        if (string.IsNullOrWhiteSpace(Text)) return "(Empty)";
        return Color == null ? Text : $"'{Text}' colored {Color}";
    }
}