using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AdventOfCode
{
	internal static class Day12
	{
		public static int Part1(string input)
		{
			return Regex.Matches(input, @"-?\d+").OfType<Match>().Select(m => int.Parse(m.Value)).Sum();
		}

		public static int Part2(string input)
		{
			JToken d = JsonConvert.DeserializeObject<JToken>(input);
			return Count(d);
		}

		private static int Count(JToken value)
		{
			if (value is JArray)
			{
				JArray arr = (JArray)value;
				int sum = 0;
				foreach (JToken val in arr)
				{
					sum += Count(val);
				}

				return sum;
			}

			if (value is JObject)
			{
				JObject obj = (JObject)value;
				if (
					obj.AsJEnumerable()
					   .OfType<JProperty>()
					   .Any(
						   jv =>
							   jv.Value is JValue && ((JValue)jv.Value).Value is string &&
							   (string)((JValue)jv.Value).Value == "red"))
					return 0;

				return obj.AsJEnumerable().Sum(Count);
			}

			if (value is JProperty)
			{
				return Count(((JProperty)value).Value);
			}

			if (value is JValue)
			{
				JValue val = (JValue)value;
				if (val.Value is long)
					return (int)(long)val.Value;

				return 0;
			}

			Debugger.Break();
			return 0;
		}
	}
}