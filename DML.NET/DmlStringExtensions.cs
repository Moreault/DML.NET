namespace ToolBX.DML.NET;

public static class DmlStringExtensions
{
    public static DmlString ToDmlString(this IEnumerable<DmlSubstring> source)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));

        var currentIndex = 0;
        return new DmlString(source.Select(x =>
        {
            var thisIndex = currentIndex;
            currentIndex += x.Text.Length;

            return new DmlSubstringEntry(x)
            {
                StartIndex = thisIndex
            };
        }));
    }
}