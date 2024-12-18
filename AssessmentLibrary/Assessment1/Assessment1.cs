namespace AssessmentLibrary.Assessment1;

public class Assessment1
{
    public IEnumerable<string> ConvertBytes(IEnumerable<byte> input)
    {
        // Using provided IntegerConverter.Convert(int value)
        // return equivalent collection of string values for each given input.
        foreach (var item in input)
        {
            yield return IntegerConverter.Convert(item);
        }
    }
    public IEnumerable<string> FilterAndConvertIntegers(IEnumerable<int> input)
    {
        // Using provided IntegerConverter.Convert(int value)
        // return equivalent collection of string values for each given input.
        // Where input value cannot be converted, remove the offending value.
        var filteredInput = input.Where(x => x >= IntegerConverter.MinValue && x <= IntegerConverter.MaxValue);
        foreach (var item in filteredInput)
        {
            yield return IntegerConverter.Convert(item);
        }
    }

    public Dictionary<string, int> CountOccurrences(IEnumerable<byte> input)
    {
        // Using provided IntegerConverter.Convert(int value)
        // return a summary of values and the occurrence count.
        var dict = new Dictionary<byte, int>();
        foreach (var item in input)
        {
            if (dict.ContainsKey(item))
            {
                dict[item]++;
            } 
            else
            {
                dict.Add(item, 1);
            }
        }

        return dict.ToDictionary(x => IntegerConverter.Convert(x.Key), x => x.Value);
    }

    public Dictionary<string, int> ExtractMostCommonBytes(IEnumerable<byte> input)
    {
        // Using provided IntegerConverter.Convert(int value)
        // return top 10 most commonly occurring values and the occurrence count ideally in less than 2seconds.
        var dict = new Dictionary<byte, int>();
        foreach (var item in input)
        {
            if (dict.ContainsKey(item))
            {
                dict[item]++;
            }
            else
            {
                dict.Add(item, 1);
            }
        }

        return dict
            .OrderByDescending(x => x.Value)
            .Take(10)
            .ToDictionary(x => IntegerConverter.Convert(x.Key), x => x.Value);
    }
}