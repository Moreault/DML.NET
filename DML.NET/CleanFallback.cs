namespace ToolBX.DML.NET;

/// <summary>
/// Determines what <see cref="DmlSubstring.Clean"/> is filled with when a <c>&lt;profanity&gt;</c> tag does not
/// provide an explicit <c>clean</c> attribute. Configured through <see cref="DmlOptions.CleanFallback"/>.
/// </summary>
public enum CleanFallback
{
    /// <summary>
    /// Fill <see cref="DmlSubstring.Clean"/> with a length-matched string of random-looking symbols (ex: "#$@!" for
    /// "damn".) The result is deterministic : the same word always produces the same grawlix. Whitespace is preserved.
    /// </summary>
    Grawlix,

    /// <summary>
    /// Fill <see cref="DmlSubstring.Clean"/> with a length-matched string of asterisks (ex: "****" for "damn".)
    /// Whitespace is preserved.
    /// </summary>
    Asterisks,

    /// <summary>
    /// Leave <see cref="DmlSubstring.Clean"/> as <c>null</c> when no <c>clean</c> attribute is provided. DML invents
    /// nothing : it is entirely up to the consuming application to decide what to display in place of the profanity.
    /// This is the default, in keeping with DML's "only a spec" philosophy.
    /// </summary>
    DoNothing
}
