namespace ToolBX.DML.NET.Conversion;

public interface IDmlTextStyleConverter
{
    IReadOnlyList<TextStyle> Convert(MetaString metaString);
}

[AutoInject]
public class DmlTextStyleConverter : IDmlTextStyleConverter
{
    public IReadOnlyList<TextStyle> Convert(MetaString metaString)
    {
        if (metaString == null) throw new ArgumentNullException(nameof(metaString));

        var styles = new List<TextStyle>();
        var bolds = metaString.Tags.Count(x => string.Equals(x.Name, DmlTags.Bold, StringComparison.CurrentCultureIgnoreCase));
        if (bolds == 1) styles.Add(TextStyle.Bold);
        else if (bolds > 1) throw new Exception(string.Format(Exceptions.CannotDeserializeDmlBecauseDuplicateTextStyle, metaString.Text, DmlTags.Bold));

        var italics = metaString.Tags.Count(x => string.Equals(x.Name, DmlTags.Italic, StringComparison.CurrentCultureIgnoreCase));
        if (italics == 1) styles.Add(TextStyle.Italic);
        else if (italics > 1) throw new Exception(string.Format(Exceptions.CannotDeserializeDmlBecauseDuplicateTextStyle, metaString.Text, DmlTags.Italic));

        var underlines = metaString.Tags.Count(x => string.Equals(x.Name, DmlTags.Underline, StringComparison.CurrentCultureIgnoreCase));
        if (underlines == 1) styles.Add(TextStyle.Underline);
        else if (underlines > 1) throw new Exception(string.Format(Exceptions.CannotDeserializeDmlBecauseDuplicateTextStyle, metaString.Text, DmlTags.Underline));

        var strikeouts = metaString.Tags.Count(x => string.Equals(x.Name, DmlTags.Strikeout, StringComparison.CurrentCultureIgnoreCase));
        if (strikeouts == 1) styles.Add(TextStyle.Strikeout);
        else if (strikeouts > 1) throw new Exception(string.Format(Exceptions.CannotDeserializeDmlBecauseDuplicateTextStyle, metaString.Text, DmlTags.Strikeout));

        return styles;
    }
}