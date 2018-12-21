#include <iostream>
#include <unordered_set>
int main()
{
	std::unordered_set<int> seen;
	int reg[6] = { 0 };
	int min = 0x7FFFFFFF, max = 0;
LBL_0: reg[2] = 123; // seti 123 0 2
LBL_1: reg[2] &= 456; // bani 2 456 2
LBL_2: reg[2] = reg[2] == 72 ? 1 : 0; // eqri 2 72 2
LBL_3: if (reg[2] == 1) { goto LBL_5; } // addr 2 5 5
LBL_4: goto LBL_1; // seti 0 0 5
LBL_5: reg[2] = 0; // seti 0 9 2
LBL_6: reg[1] = reg[2] | 65536; // bori 2 65536 1
LBL_7: reg[2] = 1250634; // seti 1250634 6 2
LBL_8: reg[4] = reg[1] & 255; // bani 1 255 4
LBL_9: reg[2] += reg[4]; // addr 2 4 2
LBL_10: reg[2] &= 16777215; // bani 2 16777215 2
LBL_11: reg[2] *= 65899; // muli 2 65899 2
LBL_12: reg[2] &= 16777215; // bani 2 16777215 2
LBL_13: reg[4] = 256 > reg[1] ? 1 : 0; // gtir 256 1 4
LBL_14: if (reg[4] == 1) { goto LBL_16; } // addr 4 5 5
LBL_15: goto LBL_17; // addi 5 1 5
LBL_16: goto LBL_28; // seti 27 2 5
LBL_17: reg[4] = 0; // seti 0 5 4
LBL_18: reg[3] = reg[4] + 1; // addi 4 1 3
LBL_19: reg[3] *= 256; // muli 3 256 3
LBL_20: reg[3] = reg[3] > reg[1] ? 1 : 0; // gtrr 3 1 3
LBL_21: if (reg[3] == 1) { goto LBL_23; } // addr 3 5 5
LBL_22: goto LBL_24; // addi 5 1 5
LBL_23: goto LBL_26; // seti 25 5 5
LBL_24: reg[4] += 1; // addi 4 1 4
LBL_25: goto LBL_18; // seti 17 2 5
LBL_26: reg[1] = reg[4]; // setr 4 8 1
LBL_27: goto LBL_8; // seti 7 6 5
LBL_28: reg[4] = reg[2] == reg[0] ? 1 : 0; // eqrr 2 0 4
	if (seen.find(reg[2]) == seen.end())
	{
		std::cout << reg[2] << std::endl;
		seen.insert(reg[2]);
	}
LBL_29: if (reg[4] == 1) { return 0; } // addr 4 5 5
LBL_30: goto LBL_6; // seti 5 7 5
}
