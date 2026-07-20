namespace ToolBX.DML.NET.Conversion;

/// <summary>
/// Result of converting a 'color' or 'highlight' tag. A DML color is either a resolved <see cref="Color"/>
/// (when the tag used a hex code or RGBA attributes) or an opaque <see cref="Name"/> (when the tag used a
/// named color.) DML does not resolve names to actual colors : it is up to the consuming application to do so.
/// </summary>
public sealed record DmlColor
{
    /// <summary>
    /// Resolved color when the tag used a hex code or RGBA attributes. <c>null</c> when a named color was used.
    /// </summary>
    public Color? Color { get; init; }

    /// <summary>
    /// Opaque color name when the tag used one (ex: "crimson".) DML hands it back untouched for the consuming
    /// application to resolve. <c>null</c> when a resolved <see cref="Color"/> was used instead.
    /// </summary>
    public string? Name { get; init; }
}
