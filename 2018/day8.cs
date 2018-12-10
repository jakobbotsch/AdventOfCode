using System;
using System.IO;
using System.Linq;

public class Program
{
    public static void Main()
    {
        int[] input = File.ReadAllText("day8.txt").Split(' ').Select(int.Parse).ToArray();
        Span<int> s = input;
        Console.WriteLine(SumMetadata(ref s));
        s = input;
        Console.WriteLine(GetValue(ref s));
    }

    private static int SumMetadata(ref Span<int> nodes)
    {
        int numChildren = nodes[0];
        int numMetadata = nodes[1];
        int result = 0;
        nodes = nodes.Slice(2);
        for (int i = 0; i < numChildren; i++)
            result += SumMetadata(ref nodes);

        result += nodes.Slice(0, numMetadata).ToArray().Sum();
        nodes = nodes.Slice(numMetadata);
        return result;
    }

    private static int GetValue(ref Span<int> nodes)
    {
        int numChildren = nodes[0];
        int numMetadata = nodes[1];
        nodes = nodes.Slice(2);
        int[] childSums = new int[numChildren];
        for (int i = 0; i < numChildren; i++)
            childSums[i] = GetValue(ref nodes);

        int[] metadata = nodes.Slice(0, numMetadata).ToArray();
        nodes = nodes.Slice(numMetadata);
        return numChildren == 0 ? metadata.Sum() : metadata.Select(i => i - 1 >= childSums.Length ? 0 : childSums[i - 1]).Sum();
    }
}
