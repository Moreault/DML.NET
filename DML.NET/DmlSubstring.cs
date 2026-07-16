namespace ToolBX.DML.NET;

public sealed record DmlSubstring
{
    /// <summary>
    /// Clean text without any DML tags.
    /// </summary>
    public string Text { get; init; } = string.Empty;

    /// <summary>
    /// Color that the text should be written in. <c>null</c> when the text uses the default color or when a named
    /// color was used instead, in which case see <see cref="ColorName"/>.
    /// </summary>
    public Color? Color { get; init; }

    /// <summary>
    /// Name of the color the text should be written in when a named color was used (ex: "crimson".) DML does not
    /// interpret this value : it is up to the consuming application to resolve it into an actual color. <c>null</c>
    /// when no named color was used (the color is either default or a resolved <see cref="Color"/>.)
    /// </summary>
    public string? ColorName { get; init; }

    /// <summary>
    /// Color that the background behind the text should be highlighted in. <c>null</c> when there is no highlight or
    /// when a named color was used instead, in which case see <see cref="HighlightName"/>.
    /// </summary>
    public Color? Highlight { get; init; }

    /// <summary>
    /// Name of the color the background should be highlighted in when a named color was used. DML does not interpret
    /// this value : it is up to the consuming application to resolve it. <c>null</c> when no named color was used.
    /// </summary>
    public string? HighlightName { get; init; }

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
        if (Color != null) return $"'{Text}' colored {Color}";
        if (!string.IsNullOrWhiteSpace(ColorName)) return $"'{Text}' colored {ColorName}";
        return Text;
    }
}