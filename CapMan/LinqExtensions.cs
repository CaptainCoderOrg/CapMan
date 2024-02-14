namespace CapMan;

public static class LinqExtensions
{
     public static IEnumerable<TSource> NotWhere<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate) => source.Where(x => !predicate(x));
}
