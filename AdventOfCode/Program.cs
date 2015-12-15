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

			bool any = false;
			for (int i = 1;; i++)
			{
				object result;
				if (RunDayMethod(@class, "Part" + i, inputPath, out result))
				{
					Console.WriteLine("Part {0}: {1}", i, result);
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

		private static bool RunDayMethod(Type @class, string methodName, string inputPath, out object result)
		{
			MethodInfo method = @class.GetMethod(methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
			if (method == null)
			{
				result = null;
				return false;
			}

			ParameterInfo[] pars = method.GetParameters();
			if (pars.Length == 0)
			{
				if (method.IsStatic)
					result = method.Invoke(null, new object[0]);
				else
				{
					object instance = Activator.CreateInstance(@class, true);
					result = method.Invoke(instance, new object[0]);
				}

				return true;
			}

			if (pars.Length != 1 || pars[0].ParameterType != typeof(string) && pars[0].ParameterType != typeof(string[]))
			{
				Console.WriteLine("{0}.{1} has unsupported parameters", @class.FullName, methodName);
				result = null;
				return false;
			}

			if (!File.Exists(inputPath))
			{
				Console.WriteLine("Cannot invoke {0}.{1} as there is no input for it", @class.FullName,
				                  methodName);
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
