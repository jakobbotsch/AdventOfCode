using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Properties;

namespace AdventOfCode
{
	internal static class Day23
	{
		public static uint Part1(string[] instructions)
		{
			var regs = new DefaultDictionary<string, uint>();
			Simulate(instructions, regs);
			return regs["b"];
		}

		public static uint Part2(string[] instructions)
		{
			var regs = new DefaultDictionary<string, uint> {["a"] = 1};
			Simulate(instructions, regs);
			return regs["b"];
		}

		private static void Simulate(string[] instructions, DefaultDictionary<string, uint> regs)
		{
			int ip = 0;
			while (ip >= 0 && ip < instructions.Length)
			{
				string ins = instructions[ip];
				if (ins.StartsWith("hlf") || ins.StartsWith("tpl") || ins.StartsWith("inc"))
				{
					string reg = ins[4].ToString();
					switch (ins.Substring(0, 3))
					{
						case "hlf":
							regs[reg] /= 2;
							break;
						case "tpl":
							regs[reg] *= 3;
							break;
						case "inc":
							regs[reg]++;
							break;
					}

					ip++;
					continue;
				}

				if (ins.StartsWith("jmp"))
				{
					ip += int.Parse(ins.Substring(4));
					continue;
				}

				if (ins.StartsWith("jie") || ins.StartsWith("jio"))
				{
					var reg = ins[4].ToString();
					int offset = int.Parse(ins.Substring(6));
					if (ins.StartsWith("jie") && regs[reg]%2 == 0)
						ip += offset;
					else if (ins.StartsWith("jio") && regs[reg] == 1)
						ip += offset;
					else
						ip++;
				}
			}
		}
	}
}