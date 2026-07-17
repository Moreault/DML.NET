namespace ToolBX.DML.NET;

/// <summary>
/// Options that tune how DML deserializes certain tags. Bind them from the "Dml" section of your appsettings via
/// <c>services.AddAutoConfig(configuration)</c>, or configure them programmatically through
/// <see cref="ServiceCollectionExtensions.AddDml"/>. When left unconfigured, the defaults below apply.
/// </summary>
[AutoConfig("Dml")]
public sealed record DmlOptions
{
    /// <summary>
    /// What <see cref="DmlSubstring.Clean"/> is filled with when a <c>&lt;profanity&gt;</c> tag has no explicit
    /// <c>clean</c> attribute. Defaults to <see cref="NET.CleanFallback.DoNothing"/> : out of the box DML invents
    /// nothing and leaves the censoring to you, in keeping with its "only a spec" philosophy. Set it to
    /// <see cref="NET.CleanFallback.Grawlix"/> or <see cref="NET.CleanFallback.Asterisks"/> to opt into an
    /// auto-generated mask.
    /// </summary>
    public CleanFallback CleanFallback { get; init; } = CleanFallback.DoNothing;

    /// <summary>
    /// The <see cref="ProfanityLevel"/> assigned to a <c>&lt;profanity&gt;</c> tag that omits its <c>level</c>
    /// attribute. Defaults to <see cref="ProfanityLevel.Strong"/>. Set to <c>null</c> if you treat all profanity
    /// equally and don't want a level assigned : <see cref="DmlSubstring.IsProfanity"/> still reports the span as
    /// profanity regardless.
    /// </summary>
    public ProfanityLevel? DefaultProfanityLevel { get; init; } = ProfanityLevel.Strong;
}
