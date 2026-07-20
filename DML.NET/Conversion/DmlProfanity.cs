namespace ToolBX.DML.NET.Conversion;

/// <summary>
/// Result of converting a 'profanity' tag. Reports whether a span is profanity, its severity and the text that
/// should be displayed in its place.
/// </summary>
public sealed record DmlProfanity
{
    /// <summary>
    /// The severity of the profanity, or <c>null</c> when no level was assigned (see
    /// <see cref="DmlOptions.DefaultProfanityLevel"/>.) A <c>null</c> level does not mean the span isn't profanity.
    /// </summary>
    public ProfanityLevel? Level { get; init; }

    /// <summary>
    /// The text to display in place of the profanity. Holds the tag's explicit <c>clean</c> attribute when present,
    /// otherwise the value produced by <see cref="DmlOptions.CleanFallback"/> (which may be <c>null</c>.)
    /// </summary>
    public string? Clean { get; init; }
}
