namespace ToolBX.DML.NET.Conversion;

public interface IDmlColorTagConverter
{
    DmlColor Convert(MarkupTag tag);
}

[AutoInject(ServiceLifetime.Singleton)]
public sealed class DmlColorTagConverter : IDmlColorTagConverter
{
    private const string HexPattern = "^#(?:[0-9a-fA-F]{3}){1,2}$";
    private const string NamePattern = "^[A-Za-z][A-Za-z0-9_-]*$";

    public DmlColor Convert(MarkupTag tag)
    {
        if (tag == null) throw new ArgumentNullException(nameof(tag));
        if (!string.Equals(tag.Name, DmlTags.Color, StringComparison.InvariantCultureIgnoreCase) && !string.Equals(tag.Name, DmlTags.Highlight, StringComparison.InvariantCultureIgnoreCase))
            throw new Exception(string.Format(Exceptions.CannotConvertBecauseTagUnsupported, DmlTags.Color, DmlTags.Highlight, tag));

        var colorAttributes = tag.Attributes.Where(x => string.Equals(x.Name, DmlTags.Red, StringComparison.InvariantCultureIgnoreCase) ||
                                                        string.Equals(x.Name, DmlTags.Green, StringComparison.InvariantCultureIgnoreCase) ||
                                                        string.Equals(x.Name, DmlTags.Blue, StringComparison.InvariantCultureIgnoreCase) ||
                                                        string.Equals(x.Name, DmlTags.Alpha, StringComparison.InvariantCultureIgnoreCase)).ToList();

        var hasValue = !string.IsNullOrWhiteSpace(tag.Value);

        if (!hasValue && !colorAttributes.Any()) throw new Exception($"Can't convert {nameof(MarkupTag)} to {nameof(Color)} : tag '{tag}' does not appear to hold any valid color information");
        if (hasValue && colorAttributes.Any()) throw new Exception($"Can't convert {nameof(MarkupTag)} to {nameof(Color)} : tag '{tag}' has both a color value and RGBA attributes but must have only one or the other.");

        if (hasValue)
        {
            if (tag.Value.StartsWith('#'))
            {
                if (!Regex.IsMatch(tag.Value, HexPattern)) throw new Exception($"Can't convert {nameof(MarkupTag)} to {nameof(Color)} : {tag.Value} is not in a valid hex color format.");
                return new DmlColor { Color = Color.FromHtml(tag.Value) };
            }

            if (!Regex.IsMatch(tag.Value, NamePattern)) throw new Exception($"Can't convert {nameof(MarkupTag)} to {nameof(Color)} : {tag.Value} is not a valid color name.");
            return new DmlColor { Name = tag.Value };
        }

        var colorAttributesDictionary = colorAttributes.ToDictionary(x => x.Name.ToLowerInvariant(), x => x.Value.ToInt());

        if (colorAttributesDictionary.Values.Any(x => !x.IsSuccess)) throw new Exception($"Can't convert {nameof(MarkupTag)} to {nameof(Color)} : tag '{tag}' contains non-numeric values.");
        if (colorAttributesDictionary.Values.Any(x => x.Value < 0 || x.Value > 255)) throw new Exception($"Can't convert {nameof(MarkupTag)} to {nameof(Color)} : tag '{tag}' values outside the accepted range of 0 and 255.");

        colorAttributesDictionary.TryGetValue(DmlTags.Red, out var red);
        colorAttributesDictionary.TryGetValue(DmlTags.Green, out var green);
        colorAttributesDictionary.TryGetValue(DmlTags.Blue, out var blue);
        colorAttributesDictionary.TryGetValue(DmlTags.Alpha, out var alpha);

        return new DmlColor { Color = new Color(red.Value, green.Value, blue.Value, alpha.IsSuccess ? alpha.Value : 255) };
    }
}
