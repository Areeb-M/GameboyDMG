using System;
using System.Collections;
using System.Collections.Generic;

namespace Emulator
{
	delegate IEnumerable<bool> OF(Memory mem, Registers reg);
	// OF = OpcodeFunction
	static class OpcodeTable
	{
        // 84/245 Opcodes Implemented
		private static Dictionary<byte, OF> table = new Dictionary<byte, OF>()
        {
            {0x00, (OF)Opcode.NOP},
            {0x01, (OF)Opcode.LOAD_N_D16},
            {0x04, (OF)Opcode.INCREMENT_REG},
            {0x05, (OF)Opcode.DECREMENT_REG},
            {0x06, (OF)Opcode.LOAD_N_D8},
            {0x0B, (OF)Opcode.DECREMENT_NN},
            {0x0C, (OF)Opcode.INCREMENT_REG},
            {0x0D, (OF)Opcode.DECREMENT_REG},
            {0x0E, (OF)Opcode.LOAD_N_D8},
            {0x11, (OF)Opcode.LOAD_N_D16},
            {0x12, (OF)Opcode.LOAD_N_N},
            {0x13, (OF)Opcode.INCREMENT_16_REG},
            {0x15, (OF)Opcode.DECREMENT_REG},
            {0x16, (OF)Opcode.LOAD_N_D8},
            {0x17, (OF)Opcode.RLA},
            {0x18, (OF)Opcode.JR_R8},
            {0x19, (OF)Opcode.ADD_HL_NN},
            {0x1A, (OF)Opcode.LOAD_N_N},
            {0x1C, (OF)Opcode.INCREMENT_REG},
            {0x1D, (OF)Opcode.DECREMENT_REG},
            {0x1E, (OF)Opcode.LOAD_N_D8},
            {0x20, (OF)Opcode.JR_CC_R8},
            {0x21, (OF)Opcode.LOAD_N_D16},
            {0x22, (OF)Opcode.LDI_HL_A},
            {0x23, (OF)Opcode.INCREMENT_16_REG},
            {0x24, (OF)Opcode.INCREMENT_REG},
            {0x28, (OF)Opcode.JR_CC_R8},
            {0x2A, (OF)Opcode.LDI_A_HL},
            {0x2E, (OF)Opcode.LOAD_N_D8},
            {0x2F, (OF)Opcode.COMPLEMENT},
            {0x31, (OF)Opcode.LOAD_N_D16},
            {0x32, (OF)Opcode.LDD_HL_A},
            {0x35, (OF)Opcode.DECREMENT_REG},
            {0x36, (OF)Opcode.LOAD_N_N},
            {0x3D, (OF)Opcode.DECREMENT_REG},
            {0x3E, (OF)Opcode.LOAD_N_D8},
            {0x47, (OF)Opcode.LOAD_N_A},
            {0x4F, (OF)Opcode.LOAD_N_A},
            {0x56, (OF)Opcode.LOAD_N_N},
            {0x57, (OF)Opcode.LOAD_N_A},
            {0x5E, (OF)Opcode.LOAD_N_N},
            {0x5F, (OF)Opcode.LOAD_N_A},
            {0x67, (OF)Opcode.LOAD_N_A},
            {0x77, (OF)Opcode.LOAD_N_A},
            {0x78, (OF)Opcode.LOAD_N_N},
            {0x79, (OF)Opcode.LOAD_N_N},
            {0x7B, (OF)Opcode.LOAD_N_N},
            {0x7C, (OF)Opcode.LOAD_N_N},
            {0x7D, (OF)Opcode.LOAD_N_N},
            {0x7E, (OF)Opcode.LOAD_N_N},
            {0x86, (OF)Opcode.ADD},
            {0x87, (OF)Opcode.ADD},
            {0x90, (OF)Opcode.SUB}, // ---------------------- 9
            {0xA1, (OF)Opcode.AND}, // ---------------------- A
            {0xA7, (OF)Opcode.AND},
            {0xA9, (OF)Opcode.XOR},
            {0xAF, (OF)Opcode.XOR},
            {0xB0, (OF)Opcode.OR}, // ---------------------- B
            {0xB1, (OF)Opcode.OR},
            {0xBE, (OF)Opcode.COMPARE},
            {0xC1, (OF)Opcode.POP},
            {0xC3, (OF)Opcode.JUMP},
            {0xC5, (OF)Opcode.PUSH},
            {0xC8, (OF)Opcode.RET_CC},
            {0xC9, (OF)Opcode.RETURN},
            {0xCA, (OF)Opcode.JP_CC_NN},
            {0xCB, (OF)Opcode.PREFIX_CB},
            {0xCD, (OF)Opcode.CALL_NN},
            {0xD1, (OF)Opcode.POP},
            {0xD5, (OF)Opcode.PUSH},
            {0xE0, (OF)Opcode.LD_FFNN_A},
            {0xE1, (OF)Opcode.POP},
            {0xE2, (OF)Opcode.LOAD_0xFFCC_A},
            {0xE5, (OF)Opcode.PUSH},
            {0xE6, (OF)Opcode.AND},
            {0xE9, (OF)Opcode.JUMP},
            {0xEA, (OF)Opcode.LOAD_N_A},
            {0xEF, (OF)Opcode.RESTART},
			{0xF0, (OF)Opcode.LOAD_A_FFNN},
            {0xF1, (OF)Opcode.POP},
            {0xF3, (OF)Opcode.DI},
            {0xF5, (OF)Opcode.PUSH},
            {0xFA, (OF)Opcode.LOAD_N_N},
            {0xFB, (OF)Opcode.EI},
			{0xFE, (OF)Opcode.COMPARE},
		};
		
		public static bool ContainsKey(byte key)
		{
			return table.ContainsKey(key);
		}
		
		public static IEnumerator<bool> Call(byte opcode, Memory mem, Registers reg)
		{
			return table[opcode](mem, reg).GetEnumerator();
		}
	}
}