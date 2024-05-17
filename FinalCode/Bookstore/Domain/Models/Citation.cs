using System.Collections;
using System.Collections.Immutable;

namespace Bookstore.Domain.Models;

public class Citation : IEnumerable<CitationSegment>
{
    public string Text => string.Concat(Segments.Select(s => s.Text));
    public bool IsEmpty => Segments.IsEmpty;
    private ImmutableList<CitationSegment> Segments { get; }

    private Citation(ImmutableList<CitationSegment> segments) => Segments = segments;
    public static Citation Empty { get; } = new(ImmutableList<CitationSegment>.Empty);

    public Citation Add(CitationSegment segment, params CitationSegment[] others) => new(Segments.Add(segment).AddRange(others));
    public Citation Add(Citation citation) => new(Segments.AddRange(citation.Segments));

    public static Citation Join(CitationSegment separator, IEnumerable<Citation> segments) =>
        Join((Citation)separator, segments);

    public static Citation Join(Citation separator, IEnumerable<Citation> segments)
    {
        Citation result = Empty;
        using IEnumerator<Citation> enumerator = segments.GetEnumerator();
        if (!enumerator.MoveNext()) return result;
        result = result.Add(enumerator.Current);

        while (enumerator.MoveNext())
            result = result.Add(separator).Add(enumerator.Current);

        return result;
    }

    public IEnumerator<CitationSegment> GetEnumerator() => Segments.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public static implicit operator Citation(CitationSegment segment) => Empty.Add(segment);
}