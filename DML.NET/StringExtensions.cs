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