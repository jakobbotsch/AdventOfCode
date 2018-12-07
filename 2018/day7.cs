using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class Program
{
    public static void Main()
    {
        string[] lines = File.ReadAllLines(@"day7.txt");
        List<(char depsOn, char node)> graphEdges = lines.Select(l => (l.Split(' ')[1][0], l.Split(' ')[7][0])).ToList();
        List<char> nodes = graphEdges.SelectMany(t => new[] { t.depsOn, t.node }).Distinct().ToList();
        List<char> left = nodes.ToList();
        string order = "";
        while (left.Count > 0)
        {
            char ready =
                left
                .Where(c => graphEdges.Where(t => t.node == c).All(t => order.Contains(t.depsOn)))
                .OrderBy(c => c)
                .First();

            order += ready;
            left.Remove(ready);
        }
        Console.WriteLine(order);

        Dictionary<char, int> started = new Dictionary<char, int>();
        HashSet<char> completed = new HashSet<char>();
        int free = 5;
        int time;
        for (time = 0; ; time++)
        {
            bool isTaskDone(KeyValuePair<char, int> startInfo)
                => time >= startInfo.Value + 60 + (startInfo.Key - 'A') + 1;

            List<char> completedNow =
                started.Where(isTaskDone).Select(kvp => kvp.Key).ToList();

            completed.UnionWith(completedNow);
            free += completedNow.Count;

            if (completed.Count >= nodes.Count)
                break;

            bool canStartTask(char c)
                => !started.ContainsKey(c) &&
                   graphEdges.Where(t => t.node == c).All(t => completed.Contains(t.depsOn));

            IEnumerable<char> avail = nodes.Where(canStartTask).OrderBy(c => c);

            foreach (char c in avail)
            {
                if (free <= 0)
                    break;
                started[c] = time;
                free--;
            }
        }

        Console.WriteLine(time);
    }
}
