namespace ToolBX.DML.NET;

public static class StringExtensions
{
    /// <summary>
    /// Surrounds the string with the 'color' DML tag 
    /// </summary>
    public static string Color(this string value, byte red, byte green, byte blue, byte alpha = byte.MaxValue) => value.Color(new Color(red, green, blue, alpha));

    /// <summary>
    /// Surrounds the string with the 'color' DML tag
    /// </summary>
    public static string Color(this string value, Color color)
    {
        return $"<{DmlTags.Color} red={color.Red} green={color.Green} blue={color.Blue} alpha={color.Alpha}>{value}</{DmlTags.Color}>";
    }

    /// <summary>
    /// Surrounds the string with the 'color' DML tag using a named color (ex: "crimson".) DML does not resolve the
    /// name : it is handed back to the consuming application untouched. A hex code (prefixed with '#') is also accepted.
    /// </summary>
    public static string Color(this string value, string name)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException($"'{nameof(name)}' cannot be null or blank.", nameof(name));
        return $"<{DmlTags.Color}={name}>{value}</{DmlTags.Color}>";
    }

    /// <summary>
    /// Surrounds the string with the 'highlight' DML tag 
    /// </summary>
    public static string Highlight(this string value, byte red, byte green, byte blue, byte alpha = byte.MaxValue) => value.Highlight(new Color(red, green, blue, alpha));

    /// <summary>
    /// Surrounds the string with the 'highlight' DML tag
    /// </summary>
    public static string Highlight(this string value, Color color)
    {
        return $"<{DmlTags.Highlight} red={color.Red} green={color.Green} blue={color.Blue} alpha={color.Alpha}>{value}</{DmlTags.Highlight}>";
    }

    /// <summary>
    /// Surrounds the string with the 'highlight' DML tag using a named color (ex: "crimson".) DML does not resolve the
    /// name : it is handed back to the consuming application untouched. A hex code (prefixed with '#') is also accepted.
    /// </summary>
    public static string Highlight(this string value, string name)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException($"'{nameof(name)}' cannot be null or blank.", nameof(name));
        return $"<{DmlTags.Highlight}={name}>{value}</{DmlTags.Highlight}>";
    }

    /// <summary>
    /// Surrounds the string with the 'keyword' DML tag, defaulting its id to the text itself when deserialized.
    /// </summary>
    public static string Keyword(this string value) => $"<{DmlTags.Keyword}>{value}</{DmlTags.Keyword}>";

    /// <summary>
    /// Surrounds the string with the 'keyword' DML tag using the keyword's id. A null or blank id falls back to
    /// the id-less form, in which case the id defaults to the text itself when deserialized.
    /// </summary>
    public static string Keyword(this string value, string id)
    {
        if (string.IsNullOrWhiteSpace(id)) return value.Keyword();
        return $"<{DmlTags.Keyword}={id}>{value}</{DmlTags.Keyword}>";
    }

    /// <summary>
    /// Surrounds the string with the 'profanity' DML tag. The level defaults to <see cref="DmlOptions.DefaultProfanityLevel"/>
    /// and, when deserialized, its clean alternative is produced according to <see cref="DmlOptions.CleanFallback"/>.
    /// </summary>
    public static string Profanity(this string value) => $"<{DmlTags.Profanity}>{value}</{DmlTags.Profanity}>";

    /// <summary>
    /// Surrounds the string with the 'profanity' DML tag using an explicit level.
    /// </summary>
    public static string Profanity(this string value, ProfanityLevel level) =>
        $"<{DmlTags.Profanity} {DmlTags.Level}={level.ToString().ToLowerInvariant()}>{value}</{DmlTags.Profanity}>";

    /// <summary>
    /// Surrounds the string with the 'profanity' DML tag using an explicit level and a clean alternative to display
    /// in place of the profanity. A null or blank clean alternative is omitted, in which case it falls back to
    /// <see cref="DmlOptions.CleanFallback"/> when deserialized.
    /// </summary>
    public static string Profanity(this string value, ProfanityLevel level, string clean)
    {
        if (string.IsNullOrWhiteSpace(clean)) return value.Profanity(level);
        return $"<{DmlTags.Profanity} {DmlTags.Level}={level.ToString().ToLowerInvariant()} {DmlTags.Clean}=\"{clean}\">{value}</{DmlTags.Profanity}>";
    }

    /// <summary>
    /// Surrounds the string with a DML text style tag
    /// </summary>
    public static string Style(this string value, TextStyle style)
    {
        switch (style)
        {
            case TextStyle.Bold:
                return value.Bold();
            case TextStyle.Italic:
                return value.Italic();
            case TextStyle.Underline:
                return value.Underline();
            case TextStyle.Strikeout:
                return value.Strikeout();
            default:
                throw new ArgumentOutOfRangeException(nameof(style), style, null);
        }
    }

    /// <summary>
    /// Surrounds the string with the 'bold' DML tag 
    /// </summary>
    public static string Bold(this string value) => $"<{DmlTags.Bold}>{value}</{DmlTags.Bold}>";

    /// <summary>
    /// Surrounds the string with the 'italic' DML tag 
    /// </summary>
    public static string Italic(this string value) => $"<{DmlTags.Italic}>{value}</{DmlTags.Italic}>";

    /// <summary>
    /// Surrounds the string with the 'strikeout' DML tag 
    /// </summary>
    public static string Strikeout(this string value) => $"<{DmlTags.Strikeout}>{value}</{DmlTags.Strikeout}>";

    /// <summary>
    /// Surrounds the string with the 'underline' DML tag 
    /// </summary>
    public static string Underline(this string value) => $"<{DmlTags.Underline}>{value}</{DmlTags.Underline}>";
}