namespace ToolBX.DML.NET;

public interface IDmlSerializer
{
    DmlString Deserialize(string text);
}

[AutoInject(ServiceLifetime.Singleton)]
public class DmlSerializer : IDmlSerializer
{
    private readonly IMarkupParser _markupParser;
    private readonly IDmlConverter _dmlConverter;

    public DmlSerializer(IMarkupParser markupParser, IDmlConverter dmlConverter)
    {
        _markupParser = markupParser;
        _dmlConverter = dmlConverter;
    }

    public DmlString Deserialize(string text)
    {
        if (string.IsNullOrWhiteSpace(text)) throw new ArgumentNullException(nameof(text));

        var metaStrings = _markupParser.Parse(text);
        return _dmlConverter.Convert(metaStrings);
    }
}