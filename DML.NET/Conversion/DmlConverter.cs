namespace ToolBX.DML.NET.Conversion;

public interface IDmlConverter
{
    DmlString Convert(IReadOnlyList<MetaString> metaStrings);
    DmlSubstring Convert(MetaString metaString);
}

[AutoInject(ServiceLifetime.Singleton)]
public sealed class DmlConverter : IDmlConverter
{
    private readonly IDmlColorTagConverter _dmlColorTagConverter;
    private readonly IDmlTextStyleConverter _dmlTextStyleConverter;
    private readonly IDmlProfanityTagConverter _dmlProfanityTagConverter;

    public DmlConverter(IDmlColorTagConverter dmlColorTagConverter, IDmlTextStyleConverter dmlTextStyleConverter, IDmlProfanityTagConverter dmlProfanityTagConverter)
    {
        _dmlColorTagConverter = dmlColorTagConverter;
        _dmlTextStyleConverter = dmlTextStyleConverter;
        _dmlProfanityTagConverter = dmlProfanityTagConverter;
    }

    public DmlString Convert(IReadOnlyList<MetaString> metaStrings)
    {
        if (metaStrings == null) throw new ArgumentNullException(nameof(metaStrings));
        return metaStrings.Select(Convert).ToDmlString();
    }

    public DmlSubstring Convert(MetaString metaString)
    {
        if (metaString == null) throw new ArgumentNullException(nameof(metaString));

        var colorTag = metaString.Tags.LastOrDefault(x => string.Equals(x.Name, DmlTags.Color, StringComparison.InvariantCultureIgnoreCase));
        var highlightTag = metaString.Tags.LastOrDefault(x => string.Equals(x.Name, DmlTags.Highlight, StringComparison.InvariantCultureIgnoreCase));
        var keywordTag = metaString.Tags.LastOrDefault(x => string.Equals(x.Name, DmlTags.Keyword, StringComparison.InvariantCultureIgnoreCase));
        var profanityTag = metaString.Tags.LastOrDefault(x => string.Equals(x.Name, DmlTags.Profanity, StringComparison.InvariantCultureIgnoreCase));

        var color = colorTag == null ? null : _dmlColorTagConverter.Convert(colorTag);
        var highlight = highlightTag == null ? null : _dmlColorTagConverter.Convert(highlightTag);
        var profanity = profanityTag == null ? null : _dmlProfanityTagConverter.Convert(profanityTag, metaString.Text);

        return new DmlSubstring
        {
            Text = metaString.Text,
            Color = color?.Color,
            ColorName = color?.Name,
            Highlight = highlight?.Color,
            HighlightName = highlight?.Name,
            Keyword = keywordTag == null ? null : string.IsNullOrWhiteSpace(keywordTag.Value) ? metaString.Text : keywordTag.Value,
            IsProfanity = profanityTag != null,
            ProfanityLevel = profanity?.Level,
            Clean = profanity?.Clean,
            Styles = _dmlTextStyleConverter.Convert(metaString)
        };
    }
}