namespace ToolBX.DML.NET.Conversion;

public interface IDmlConverter
{
    DmlString Convert(IReadOnlyList<MetaString> metaStrings);
    DmlSubstring Convert(MetaString metaString);
}

[AutoInject(ServiceLifetime.Singleton)]
public class DmlConverter : IDmlConverter
{
    private readonly IDmlColorTagConverter _dmlColorTagConverter;
    private readonly IDmlTextStyleConverter _dmlTextStyleConverter;

    public DmlConverter(IDmlColorTagConverter dmlColorTagConverter, IDmlTextStyleConverter dmlTextStyleConverter)
    {
        _dmlColorTagConverter = dmlColorTagConverter;
        _dmlTextStyleConverter = dmlTextStyleConverter;
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

        return new DmlSubstring
        {
            Text = metaString.Text,
            Color = colorTag == null ? null : _dmlColorTagConverter.Convert(colorTag),
            Highlight = highlightTag == null ? null : _dmlColorTagConverter.Convert(highlightTag),
            Styles = _dmlTextStyleConverter.Convert(metaString)
        };
    }
}