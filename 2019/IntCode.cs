using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace AdventOfCode
{
    internal static class IntCode
    {
        public static Task RunAsync(long[] prog, ChannelReader<long> inputs, ChannelWriter<long> outputs)
        {
            long[] mem = new long[1024 * 20];
            Array.Copy(prog, 0, mem, 0, prog.Length);
            return RunAsyncWithMem(mem, inputs, outputs);
        }

        public static async Task RunAsyncWithMem(long[] mem, ChannelReader<long> inputs, ChannelWriter<long> outputs)
        {
            long ip = 0;
            long relBase = 0;
            while (true)
            {
                ref long GetParam(long index)
                {
                    ref long fst = ref mem[ip + 1 + index];
                    int divisor = 1;
                    for (long i = 0; i < index; i++)
                        divisor *= 10;
                    long mode = (mem[ip] / 100) / divisor % 10;
                    if (mode == 0)
                        return ref mem[fst];
                    if (mode == 1)
                        return ref fst;
                    Trace.Assert(mode == 2);
                    return ref mem[relBase + fst];
                }

                switch (mem[ip] % 100)
                {
                    case 1:
                        GetParam(2) = GetParam(0) + GetParam(1);
                        ip += 4;
                        break;
                    case 2:
                        GetParam(2) = GetParam(0) * GetParam(1);
                        ip += 4;
                        break;
                    case 3:
                        long input = await inputs.ReadAsync();
                        GetParam(0) = input;
                        ip += 2;
                        break;
                    case 4:
                        await outputs.WriteAsync(GetParam(0));
                        ip += 2;
                        break;
                    case 5:
                        if (GetParam(0) != 0)
                            ip = GetParam(1);
                        else
                            ip += 3;
                        break;
                    case 6:
                        if (GetParam(0) == 0)
                            ip = GetParam(1);
                        else
                            ip += 3;
                        break;
                    case 7:
                        GetParam(2) = GetParam(0) < GetParam(1) ? 1 : 0;
                        ip += 4;
                        break;
                    case 8:
                        GetParam(2) = GetParam(0) == GetParam(1) ? 1 : 0;
                        ip += 4;
                        break;
                    case 9:
                        relBase += GetParam(0);
                        ip += 2;
                        break;
                    case 99:
                        outputs.Complete();
                        return;
                    default:
                        Trace.Fail("Oh shit " + mem[ip]);
                        break;
                }
            }
        }

        public static string ToCCode(long[] program)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("#include <stdint.h>");
            sb.AppendLine("#include <inttypes.h>");
            sb.AppendLine("int main()");
            sb.AppendLine("{");
            sb.AppendLine("int64_t input;");
            sb.AppendLine("int64_t mem[16*1024] = {" + string.Join(",", program) + "};");
            sb.AppendLine("int64_t* base = mem;");
            sb.AppendLine("goto IP0;");
            HashSet<int> visited = new HashSet<int>();
            Trace(0);
            void Trace(int ip)
            {
                if (!visited.Add(ip))
                    return;

                int GetMode(int index)
                    => (int)program[ip] / (int)Math.Pow(10, index + 1) % 10;
                string GetOper(int index)
                {
                    int mode = GetMode(index);
                    if (mode == 0)
                        return $"mem[mem[{ip + 1 + index}]]";
                    if (mode == 1)
                        return $"mem[{ip + 1 + index}]";
                    System.Diagnostics.Trace.Assert(mode == 2);
                    return $"base[mem[{ip + 1 + index}]]";
                }
                long GetOperConst(int index)
                    => GetMode(index) switch
                    {
                        0 => program[program[ip + 1 + index]],
                        1 => program[ip + 1 + index],
                        var x => throw new Exception("Unhandled " + x),
                    };
                sb.Append($"IP{ip}: ");
                switch (program[ip] % 100)
                {
                    case 1:
                        sb.AppendFormat("{0} = {1} + {2};", GetOper(0), GetOper(1), GetOper(2)).AppendLine();
                        sb.AppendFormat("goto IP{0};", ip + 4).AppendLine();
                        Trace(ip + 4);
                        break;
                    case 2:
                        sb.AppendFormat("{0} = {1} * {2};", GetOper(0), GetOper(1), GetOper(2)).AppendLine();
                        sb.AppendFormat("goto IP{0};", ip + 4).AppendLine();
                        Trace(ip + 4);
                        break;
                    case 3:
                        sb.AppendLine("scanf(\"%\" PRId64, &input);");
                        sb.AppendFormat("{0} = input;", GetOper(0)).AppendLine();
                        sb.AppendFormat("goto IP{0};", ip + 2).AppendLine();
                        Trace(ip + 2);
                        break;
                    case 4:
                        sb.AppendFormat("printf(\"%\" PRId64 \"\\n\", {0});", GetOper(0)).AppendLine();
                        sb.AppendFormat("goto IP{0};", ip + 2).AppendLine();
                        Trace(ip + 2);
                        break;
                    case 5:
                        sb.AppendFormat("if ({0} != 0) {{ goto IP{1}; }}", GetOper(0), GetOper(1)).AppendLine();
                        sb.AppendFormat("goto IP{0};", ip + 3).AppendLine();
                        //Trace(checked((int)GetOperConst(1)));
                        Trace(ip + 3);
                        break;
                    case 6:
                        sb.AppendFormat("if ({0} == 0) {{ goto IP{1}; }}", GetOper(0), GetOper(1)).AppendLine();
                        sb.AppendFormat("goto IP{0};", ip + 3).AppendLine();
                        //Trace(checked((int)GetOperConst(1)));
                        Trace(ip + 3);
                        break;
                    case 7:
                        sb.AppendFormat("{0} = {1} < {2} ? 1 : 0;", GetOper(2), GetOper(0), GetOper(1)).AppendLine();
                        sb.AppendFormat("goto IP{0};", ip + 4).AppendLine();
                        Trace(ip + 4);
                        break;
                    case 8:
                        sb.AppendFormat("{0} = {1} == {2} ? 1 : 0;", GetOper(2), GetOper(0), GetOper(1)).AppendLine();
                        sb.AppendFormat("goto IP{0};", ip + 4).AppendLine();
                        Trace(ip + 4);
                        break;
                    case 9:
                        sb.AppendFormat("base += {0};", GetOper(0)).AppendLine();
                        sb.AppendFormat("goto IP{0};", ip + 2).AppendLine();
                        Trace(ip + 2);
                        break;
                    case 99:
                        sb.AppendLine("return 0;");
                        break;
                    default:
                        throw new Exception("Invalid opcode " + program[ip] % 100);
                }
            }

            return sb.ToString();
        }
    }
}
