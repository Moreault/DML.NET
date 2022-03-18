namespace ToolBX.DML.NET;

public class DmlString : IReadOnlyList<DmlSubstringEntry>, IEquatable<DmlString>
{
    private readonly IReadOnlyList<DmlSubstringEntry> _items;

    public DmlString()
    {
        _items = new List<DmlSubstringEntry>();
    }

    public DmlString(IEnumerable<DmlSubstringEntry> items)
    {
        if (items == null) throw new ArgumentNullException(nameof(items));
        _items = items.ToList();

        //TODO Make sure there are no StartIndex duplicates!
        //TODO Also make sure there are no inconsistencies such as starting indexes starting inside other substrings
        //TODO Also also make sure they're ordered by starting index
    }

    public IEnumerator<DmlSubstringEntry> GetEnumerator() => _items.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public int Count => _items.Count;

    public DmlSubstringEntry this[int index] => _items[index];

    public char this[int subStringIndex, int characterIndex] => _items[subStringIndex].Text[characterIndex];

    public DmlString Substring(int startingIndex) => Substring(startingIndex, _items.Last().EndIndex - startingIndex);

    public DmlString Substring(int startingIndex, int length)
    {
        if (startingIndex < 0) throw new ArgumentOutOfRangeException(nameof(startingIndex));
        if (length < 0) throw new ArgumentOutOfRangeException(nameof(length));
        if (length == 0) return new DmlString();

        var start = GetDmlIndex(startingIndex);
        var end = GetDmlIndex(startingIndex + length);

        var newstrings = new List<DmlSubstring>();

        if (start.Outer == end.Outer)
            return new List<DmlSubstring>
            {
                new()
                {
                    Text = _items[start.Outer].Text.Substring(start.Inner, end.Inner - start.Inner),
                    Color = _items[start.Outer].Color
                }
            }.ToDmlString();

        for (var i = start.Outer; i <= end.Outer; i++)
        {
            var item = _items[i];

            if (i > start.Outer && i < end.Outer)
            {
                newstrings.Add(item);
            }
            else if (i == start.Outer)
            {
                newstrings.Add(new DmlSubstring
                {
                    Text = item.Text.Substring(start.Inner),
                    Color = item.Color
                });
            }
            else if (i == end.Outer)
            {
                newstrings.Add(new DmlSubstring
                {
                    Text = item.Text.Substring(0, end.Inner),
                    Color = item.Color
                });
            }
        }

        return newstrings.ToDmlString();
    }

    private DmlStringIndex GetDmlIndex(int fullStringIndex)
    {
        for (var i = 0; i < _items.Count; i++)
        {
            if (_items[i].StartIndex <= fullStringIndex && _items[i].EndIndex >= fullStringIndex)
            {
                return new DmlStringIndex
                {
                    Outer = i,
                    Inner = fullStringIndex - _items[i].StartIndex
                };
            }
        }

        throw new Exception($"Can't get {nameof(DmlStringIndex)} from full string index {fullStringIndex}.");
    }

    public bool Equals(DmlString? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return _items == null && other._items == null || _items != null && other._items != null && _items.SequenceEqual(other._items);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((DmlString)obj);
    }

    public override int GetHashCode()
    {
        return (_items != null ? _items.GetHashCode() : 0);
    }

    public static bool operator ==(DmlString a, DmlString b) => a is null && b is null || a is not null && a.Equals(b);

    public static bool operator !=(DmlString a, DmlString b) => !(a == b);

    public override string ToString() => string.Join("", _items.Select(x => x.Text));
}