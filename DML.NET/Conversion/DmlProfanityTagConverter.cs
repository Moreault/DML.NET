namespace ToolBX.DML.NET.Conversion;

public interface IDmlProfanityTagConverter
{
    DmlProfanity Convert(MarkupTag tag, string text);
}

[AutoInject(ServiceLifetime.Singleton)]
public sealed class DmlProfanityTagConverter : IDmlProfanityTagConverter
{
    /// <summary>
    /// Deliberately kept short and free of letters/digits so the mask never reads like real text.
    /// </summary>
    private const string GrawlixSymbols = "!@#$%^&*?";

    private readonly DmlOptions _options;

    public DmlProfanityTagConverter(IOptions<DmlOptions> options)
    {
        _options = options.Value;
    }

    public DmlProfanity Convert(MarkupTag tag, string text)
    {
        ArgumentNullException.ThrowIfNull(tag);
        ArgumentNullException.ThrowIfNull(text);
        if (!string.Equals(tag.Name, DmlTags.Profanity, StringComparison.InvariantCultureIgnoreCase))
            throw new Exception(string.Format(Exceptions.CannotConvertBecauseTagUnsupported, DmlTags.Profanity, DmlTags.Profanity, tag));

        return new DmlProfanity
        {
            Level = ResolveLevel(tag),
            Clean = ResolveClean(tag, text)
        };
    }

    private ProfanityLevel? ResolveLevel(MarkupTag tag)
    {
        var levelAttribute = tag.Attributes.LastOrDefault(x => string.Equals(x.Name, DmlTags.Level, StringComparison.InvariantCultureIgnoreCase));
        if (levelAttribute == null || string.IsNullOrWhiteSpace(levelAttribute.Value)) return _options.DefaultProfanityLevel;

        if (!Enum.TryParse<ProfanityLevel>(levelAttribute.Value, true, out var level) || !Enum.IsDefined(level))
            throw new Exception(string.Format(Exceptions.CannotConvertBecauseInvalidProfanityLevel, levelAttribute.Value));

        return level;
    }

    private string? ResolveClean(MarkupTag tag, string text)
    {
        var cleanAttribute = tag.Attributes.LastOrDefault(x => string.Equals(x.Name, DmlTags.Clean, StringComparison.InvariantCultureIgnoreCase));
        if (cleanAttribute != null && !string.IsNullOrWhiteSpace(cleanAttribute.Value)) return cleanAttribute.Value;

        return _options.CleanFallback switch
        {
            CleanFallback.Grawlix => Mask(text, i => GrawlixSymbols[GrawlixIndex(text[i], i)]),
            CleanFallback.Asterisks => Mask(text, _ => '*'),
            _ => null
        };
    }

    /// <summary>
    /// Builds a length-matched mask of <paramref name="text"/>, preserving whitespace so word boundaries survive.
    /// </summary>
    private static string Mask(string text, Func<int, char> symbol)
    {
        if (text.Length == 0) return string.Empty;

        return string.Create(text.Length, text, (span, source) =>
        {
            for (var i = 0; i < source.Length; i++)
                span[i] = char.IsWhiteSpace(source[i]) ? source[i] : symbol(i);
        });
    }

    /// <summary>
    /// Deterministically picks a grawlix symbol for the character at <paramref name="index"/>. Mixing the character
    /// value with its position keeps the mask varied while remaining stable for a given word.
    /// </summary>
    private static int GrawlixIndex(char character, int index) => (character * 31 + index * 7) % GrawlixSymbols.Length;
}
