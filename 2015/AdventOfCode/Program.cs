using System;
using System.Diagnostics;
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

			bool any = false;
			for (int i = 1;; i++)
			{
				object result;
				TimeSpan time;
				if (RunDayMethod(@class, "Part" + i, inputPath, out result, out time))
				{
					Console.WriteLine("Part {0}:", i);
					Console.WriteLine("Time: {0:F2} ms", time.TotalMilliseconds);
					Console.WriteLine("Result: {0}", result);
					Console.WriteLine();
					any = true;
				}
				else
				{
					break;
				}
			}

			if (!any)
				Console.WriteLine("Could not execute any parts for day {0}", day);
		}

		private static bool RunDayMethod(Type @class, string methodName, string inputPath,
		                                 out object result, out TimeSpan time)
		{
			MethodInfo method = @class.GetMethod(methodName,
			                                     BindingFlags.Public | BindingFlags.NonPublic |
			                                     BindingFlags.Static | BindingFlags.Instance);
			if (method == null)
			{
				result = null;
				time = TimeSpan.Zero;
				return false;
			}

			Func<object> toRun;
			ParameterInfo[] pars = method.GetParameters();
			if (pars.Length == 0)
			{
				if (method.IsStatic)
					toRun = () => method.Invoke(null, new object[0]);
				else
				{
					object instance = Activator.CreateInstance(@class, true);
					toRun = () => method.Invoke(instance, new object[0]);
				}
			}
			else if (pars.Length == 1 &&
			         (pars[0].ParameterType == typeof(string) || pars[0].ParameterType == typeof(string[])))
			{
				if (!File.Exists(inputPath))
				{
					Console.WriteLine("Cannot invoke {0}.{1} as there is no input for it", @class.FullName,
					                  methodName);
					result = null;
					time = TimeSpan.Zero;
					return false;
				}

				object input = pars[0].ParameterType == typeof(string)
					? (object)File.ReadAllText(inputPath)
					: File.ReadAllLines(inputPath);

				if (method.IsStatic)
				{
					toRun = () => method.Invoke(null, new[] {input});
				}
				else
				{
					object instance = Activator.CreateInstance(@class, true);
					toRun = () => method.Invoke(instance, new[] {input});
				}
			}
			else
			{
				Console.WriteLine("{0}.{1} has unsupported parameters", @class.FullName, methodName);
				result = null;
				time = TimeSpan.Zero;
				return false;
			}

			Stopwatch timer = Stopwatch.StartNew();
			result = toRun();
			time = timer.Elapsed;
			return true;
		}
	}
}