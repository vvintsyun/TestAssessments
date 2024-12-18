namespace AssessmentLibrary.Assessment2;

public class Assessment2
{
    public static Dictionary<int, List<int>> PartitionList(IEnumerable<int> source, int segmentLength)
    {
        // Partition the source into smaller lists - each containing no more than segmentLength items.
        // Return a dictionary of these smaller lists indexed by segment number (starting at 1 not 0) of the source stream.
        var result = new Dictionary<int, List<int>>();

        var toSkip = 0;
        var pageNumber = 1;
        while(toSkip < source.Count())
        {
            var list = source
                .Skip(toSkip)
                .Take(segmentLength)
                .ToList();
            result.Add(pageNumber++, list);
            toSkip += segmentLength;
        }

        return result;
    }
}