using System;
using System.Collections.Generic;
using System.Linq;

public class Program
{
    public static void Main()
    {
        const int input = 652601;
        List<int> scores = new List<int> { 3, 7 };
        int e1 = 0;
        int e2 = 1;
        while (scores.Count < input + 10)
        {
            string digits = (scores[e1] + scores[e2]).ToString();
            foreach (char c in digits)
                scores.Add(c - '0');
            e1 = (e1 + scores[e1] + 1) % scores.Count;
            e2 = (e2 + scores[e2] + 1) % scores.Count;
        }

        Console.WriteLine(string.Join("", scores.Skip(input).Take(10)));

        int[] inputSeq = input.ToString().Select(c => c - '0').ToArray();
        scores.RemoveRange(2, scores.Count - 2);
        e1 = 0;
        e2 = 1;
        int checkIndex = 0;
        while (true)
        {
            string digits = (scores[e1] + scores[e2]).ToString();
            foreach (char c in digits)
                scores.Add(c - '0');
            e1 = (e1 + scores[e1] + 1) % scores.Count;
            e2 = (e2 + scores[e2] + 1) % scores.Count;

            while (checkIndex + inputSeq.Length <= scores.Count)
            {
                if (Match(checkIndex))
                {
                    Console.WriteLine(checkIndex);
                    return;
                }

                checkIndex++;
            }

            bool Match(int start)
            {
                for (int i = 0; i < inputSeq.Length; i++)
                {
                    if (scores[start + i] != inputSeq[i])
                        return false;
                }

                return true;
            }
        }
    }
}
