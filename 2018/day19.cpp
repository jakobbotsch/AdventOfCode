#include <iostream>
int main()
{
	int reg[6] = { 0 };
	reg[0] = 1;
LBL_0: goto LBL_17; // addi 5 16 5
LBL_1: reg[3] = 1; // seti 1 0 3
LBL_2: reg[2] = 1; // seti 1 2 2
LBL_3: reg[4] = reg[3] * reg[2]; // mulr 3 2 4
LBL_4: reg[4] = reg[4] == reg[1] ? 1 : 0; // eqrr 4 1 4
LBL_5: if (reg[4] == 1) { goto LBL_7; } // addr 4 5 5
LBL_6: goto LBL_8; // addi 5 1 5
LBL_7: reg[0] += reg[3]; // addr 3 0 0
LBL_8: reg[2] += 1; // addi 2 1 2
LBL_9: reg[4] = reg[2] > reg[1] ? 1 : 0; // gtrr 2 1 4
LBL_10: if (reg[4] == 1) { goto LBL_12; } // addr 5 4 5
LBL_11: goto LBL_3;
LBL_12: reg[3] += 1; // addi 3 1 3
LBL_13: reg[4] = reg[3] > reg[1] ? 1 : 0; // gtrr 3 1 4
LBL_14: if (reg[4] == 1) { goto LBL_16; } // addr 4 5 5
LBL_15: goto LBL_2; // seti 1 3 5
LBL_16: goto LBL_DONE; // mulr 5 5 5
LBL_17: reg[1] += 2; // addi 1 2 1
LBL_18: reg[1] *= reg[1]; // mulr 1 1 1
LBL_19: reg[1] *= 19; // mulr 5 1 1
LBL_20: reg[1] *= 11; // muli 1 11 1
LBL_21: reg[4] += 7; // addi 4 7 4
LBL_22: reg[4] *= 22; // mulr 4 5 4
LBL_23: reg[4] += 20; // addi 4 20 4
LBL_24: reg[1] += reg[4]; // addr 1 4 1
LBL_25: if (reg[0] == 1) { goto LBL_27; } else if (reg[0] == 0) {} else { } // addr 5 0 5
LBL_26: goto LBL_1; // seti 0 4 5
LBL_27: reg[4] = 27; // setr 5 9 4
LBL_28: reg[4] *= 28; // mulr 4 5 4
LBL_29: reg[4] += 29; // addr 5 4 4
LBL_30: reg[4] *= 30; // mulr 5 4 4
LBL_31: reg[4] *= 14; // muli 4 14 4
LBL_32: reg[4] *= 32; // mulr 4 5 4
LBL_33: reg[1] += reg[4]; // addr 1 4 1
LBL_34:reg[0] = 0; // seti 0 2 0
LBL_35: goto LBL_1; // seti 0 5 5
	LBL_DONE : std::cout << reg[0] << std::endl;
		std::cin.get();
}