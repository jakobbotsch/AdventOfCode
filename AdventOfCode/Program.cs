using System;
using System.IO;
using System.Reflection;

namespace AdventOfCode
{
	internal class Program
	{
		internal static void Main(string[] args)
		{
			Run(args);
			Console.ReadLine();
		}

		private static void Run(string[] args)
		{
			int day;
			if (args.Length > 0)
			{
				if (!int.TryParse(args[0], out day))
				{
					Console.WriteLine("Could not parse day from {0}", args[0]);
					return;
				}
			}
			else
			{
				day = DateTime.Now.Day;
			}

			Type @class = Assembly.GetExecutingAssembly().GetType("AdventOfCode.Day" + day, false);
			if (@class == null)
			{
				Console.WriteLine("Day {0} is not yet solved", day);
				return;
			}

			string inputPath = $"Input\\Day{day}.txt";
			if (!File.Exists(inputPath))
			{
				Console.WriteLine("Day {0} has no input", day);
				return;
			}

			object part1Result;
			bool any = false;
			if (RunDayMethod(@class, "Part1", inputPath, out part1Result))
			{
				Console.WriteLine("Part 1: {0}", part1Result);
				any = true;
			}

			object part2Result;
			if (RunDayMethod(@class, "Part2", inputPath, out part2Result))
			{
				Console.WriteLine("Part 2: {0}", part2Result);
				any = true;
			}

			if (!any)
				Console.WriteLine("Could not execute any parts for day {0}", day);
		}

		private static bool RunDayMethod(Type @class, string methodName, string inputPath, out object result)
		{
			MethodInfo method = @class.GetMethod(methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
			if (method == null)
			{
				result = null;
				return false;
			}

			ParameterInfo[] pars = method.GetParameters();
			if (pars.Length != 1 || pars[0].ParameterType != typeof(string) && pars[0].ParameterType != typeof(string[]))
			{
				Console.WriteLine("{0}.{1} has unsupported parameters", @class.FullName, methodName);
				result = null;
				return false;
			}

			Func<object, object> invoke = input =>
			                              {
				                              if (method.IsStatic)
					                              return method.Invoke(null, new[] {input});

				                              object instance = Activator.CreateInstance(@class, true);
				                              return method.Invoke(instance, new[] {input});
			                              };

			if (pars[0].ParameterType == typeof(string))
				result = invoke(File.ReadAllText(inputPath));
			else
				result = invoke(File.ReadAllLines(inputPath));

			return true;
		}
	}
}
