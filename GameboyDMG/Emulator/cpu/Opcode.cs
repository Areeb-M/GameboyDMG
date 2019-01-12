using System;
using System.Collections;
using System.Collections.Generic;


namespace Emulator
{	
	static class Opcode
	{
		private class ZMath // Short for Z80 Math
		{
			public static bool CheckHalfCarry(int a, int b) // (a) + (b)
			{
				return (((a&0xF) + (b&0xF)) & 0x10) == 0x10;
			}		
			
			public static bool CheckHalfBorrow(int a, int b) // (a) - (b)
			{
				return (a & 0xF) > (b & 0xF);
			}
			
		}		
	
		public static IEnumerable<bool> NOP(Memory mem, Registers reg) // 0x00
		{
			Debug.Log(0, "NOP");
			reg.PC += 1;
			
			yield break;
		}
		
		public static IEnumerable<bool> LOAD_N_D16(Memory mem, Registers reg)
		{						
			// 16 bit load
			byte high = mem[reg.PC + 2];
			byte low = mem[reg.PC + 1];	
			
			switch(mem[reg.PC])
			{
                case 0x01:
                    reg.C = low;
                    yield return true;
                    reg.B = high;
                    yield return true;
                    Debug.Log(0, "LD BC, d16");
                    break;
				case 0x11:
					reg.E = low;
					yield return true;
					reg.D = high;
					yield return true;
					Debug.Log(0, "LD DE, d16");
					break;	
				case 0x21:
					reg.L = low;
					yield return true;
					reg.H = high;
					yield return true;
					Debug.Log(0, "LD HL, d16");
					break;					
				case 0x31:
					reg.P = low;
					yield return true;
					reg.S = high;
					yield return true;
					Debug.Log(0, "LD SP, d16");
					break;
			}			
			reg.PC += 3;
			
			yield break;
		}
		
		public static IEnumerable<bool> XOR(Memory mem, Registers reg)
		{
			int a = reg.A;
			int b;
			switch (mem[reg.PC])
            {
                case 0xA9:
                    b = reg.C;
                    Debug.Log(0, "XOR C");
                    break;
                case 0xAE:
					b = mem[reg.HL];
                    Debug.Log(0, "XOR (HL)");
					break;
				case 0xAF:
					b = reg.A;
					Debug.Log(0, "XOR A");
					break;
				default:
					Debug.Log(0, "\n[Error]Unimplemented XOR opcode detected!");
					yield break;
			}
			reg.A = (byte)(a ^ b);
			reg.fZ = reg.A == 0;
			reg.fN = false;
			reg.fH = false;
			reg.fC = false;
			reg.PC += 1;
			
			yield break;
		}
		
		public static IEnumerable<bool> LDD_HL_A(Memory mem, Registers reg) // LDD (HL), A
		{
			mem[reg.HL] = reg.A;
			yield return true;
			
			Debug.Log(0, "LDD (HL), A");
			//Console.WriteLine("{0:X4}: {1:X2}", reg.HL, reg.A);
			reg.PC += 1;
			reg.HL -= 1;
			
			yield break;
		}
		
		public static IEnumerable<bool> PREFIX_CB(Memory mem, Registers reg)
		{
			yield return true;
			foreach(bool b in PrefixCB.HandleCB(mem, reg))
			{
				yield return b;
			}
			
			reg.PC += 2;
			
			yield break;
		}
		
		public static IEnumerable<bool> JR_CC_R8(Memory mem, Registers reg)
		{
			int n = (sbyte)mem[reg.PC + 1];
			yield return true;
			
			switch(mem[reg.PC])
			{
				case 0x20:
					Debug.Log(0, "JR NZ, r8");
					if (!reg.fZ)
					{
						reg.PC += n;
						yield return true;
					}
					break;
				case 0x28:
					Debug.Log(0, "JR Z, r8");
					if (reg.fZ)
					{
						reg.PC += n;
						yield return true;
					}
					break;
			}
			reg.PC += 2;
			
			yield break;
		}
		
		public static IEnumerable<bool> LOAD_N_D8(Memory mem, Registers reg)
		{
			byte n = mem[reg.PC + 1];
			yield return true;
			
			switch(mem[reg.PC])
			{
				case 0x06:
					Debug.Log(0, "LD B, d8");
					reg.B = n;
					break;
				case 0x0E:
					Debug.Log(0, "LD C, d8");
					reg.C = n;
					break;
                case 0x16:
                    Debug.Log(0, "LD D, d8");
                    reg.D = n;
                    break;
				case 0x1E:
					Debug.Log(0, "LD E, d8");
					reg.E = n;
					break;
				case 0x2E:
					Debug.Log(0, "LD L, d8");
					reg.L = n;
					break;
				case 0x3E:
					Debug.Log(0, "LD A, d8");
					reg.A = n;
					break;
			}
			reg.PC += 2;
			
			yield break;
		}
		
		public static IEnumerable<bool> LOAD_0xFFCC_A(Memory mem, Registers reg)
		{
			Debug.Log(0, "LD ($FF00+C), A");
			mem[0xFF00 + reg.C] = reg.A;
			yield return true;
			
			reg.PC += 1;
			
			yield break;
		}
		
		public static IEnumerable<bool> INCREMENT_REG(Memory mem, Registers reg)
		{
			byte val;
			switch(mem[reg.PC])
			{
				case 0x04:
					val = reg.B;
					reg.B += 1;
					Debug.Log(0, "INC B");
					break;
				case 0x0C:
					val = reg.C;
					reg.C += 1;
					Debug.Log(0, "INC C");
					break;
				case 0x24:
					val = reg.H;
					reg.H += 1;
					Debug.Log(0, "INC H");
					break;
				default:
					throw new InvalidOperationException("Increment instruction has not been implemented yet!");
					
			}
			reg.fZ = (reg.C == 0) ? true : false;
			reg.fN = false;
			reg.fH = ZMath.CheckHalfCarry(val, 1);
			reg.PC += 1;
			
			yield break;
		}
		
		public static IEnumerable<bool> LOAD_N_A(Memory mem, Registers reg)
		{
			switch(mem[reg.PC])
			{
                case 0x47:
                    reg.B = reg.A;
                    Debug.Log(0, "LD B, A");
                    break;
				case 0x4F:
					reg.C = reg.A;
					Debug.Log(0, "LD C, A");
					break;
                case 0x57:
                    reg.D = reg.A;
                    Debug.Log(0, "LD D, A");
                    break;
                case 0x5F:
                    reg.E = reg.A;
                    Debug.Log(0, "LD E, A");
                    break;
                case 0x67:
					reg.H = reg.A;
					Debug.Log(0, "LD H, A");
					break;
				case 0x77:
					mem[reg.HL] = reg.A;
					Debug.Log(0, "LD HL, A");
					yield return true;
					break;
				case 0xEA:
					byte lower = mem[reg.PC + 1];
					yield return true;
					byte upper = mem[reg.PC + 2];
					yield return true;
					mem[(upper << 8) | lower] = reg.A;
					Debug.Log(0, "LD (d16), A");
					reg.PC += 2;
					break;
			}
			
			reg.PC += 1;
			
			yield break;
		}
		
		public static IEnumerable<bool> LD_FFNN_A(Memory mem, Registers reg)
		{
			byte n = mem[reg.PC + 1];
			yield return true;
			
			mem[0xFF00 + n] = reg.A;
			yield return true;
			
			Debug.Log(0, "LD (&FF00 + n), A");
			reg.PC += 2;
			
			yield break;
		}
		
		public static IEnumerable<bool> LOAD_N_N(Memory mem, Registers reg)
		{
			switch(mem[reg.PC])
			{
				case 0x1A:
					reg.A = mem[reg.DE];
					Debug.Log(0, "LD A, (DE)");
					yield return true;
					break;
                case 0x36:
                    reg.PC += 1;
                    yield return true;
                    mem[reg.HL] = mem[reg.PC];
                    Debug.Log(0, "LD (LH), d8");
                    yield return true;
                    break;
                case 0x56:
                    reg.D = mem[reg.HL];
                    Debug.Log(0, "LD D, (HL)");
                    yield return true;
                    break;
                case 0x5E:
                    reg.E = mem[reg.HL];
                    Debug.Log(0, "LD E, (HL)");
                    yield return true;
                    break;
                case 0x78:
                    reg.A = reg.B;
                    Debug.Log(0, "LD A, B");
                    break;
                case 0x79:
                    reg.A = reg.C;
                    Debug.Log(0, "LD A, C");
                    break;
                case 0x7B:
					reg.A = reg.E;
					Debug.Log(0, "LD A, E");
					break;
                case 0x7C:
                    reg.A = reg.H;
                    Debug.Log(0, "LD A, H");
                    break;
                case 0x7D:
                    reg.A = reg.L;
                    Debug.Log(0, "LD A, L");
                    break;
            }
			
			reg.PC += 1;
			
			yield break;
		}
		
		public static IEnumerable<bool> CALL_NN(Memory mem, Registers reg)
		{
			byte low = mem[reg.PC + 1];
			yield return true;
			
			byte high = mem[reg.PC + 2];
			yield return true;
			
			reg.SP -= 1;
			yield return true; // There is no documentation on how this instruction works internally
							   // So this is mostly the product of guesswork 	
			int address = reg.PC + 3;		
			mem[reg.SP] = (byte)((address & 0xFF00) >> 8);
			yield return true;
			
			mem[--reg.SP] = (byte)(address & 0xFF);
			yield return true;
			
			Debug.Log(0, "CALL d16");
			reg.PC = (high << 8) | low;		
			yield break;			
		}
		
		public static IEnumerable<bool> PUSH(Memory mem, Registers reg)
		{
			yield return true;
			switch(mem[reg.PC])
			{
				case 0xC5:
					mem[--reg.SP] = reg.B;
					yield return true;					
					mem[--reg.SP] = reg.C;
					yield return true;
					Debug.Log(0, "PUSH BC");
					break;
			}			
			reg.PC += 1;
			
			yield break;
		}
		
		public static IEnumerable<bool> POP(Memory mem, Registers reg)
		{
			switch(mem[reg.PC])
            {
                case 0xC1:
                    reg.C = mem[reg.SP++];
                    yield return true;
                    reg.B = mem[reg.SP++];
                    yield return true;
                    Debug.Log(0, "POP BC");
                    break;
                case 0xE1:
                    reg.L = mem[reg.SP++];
                    yield return true;
                    reg.H = mem[reg.SP++];
                    yield return true;
                    Debug.Log(0, "POP HL");
                    break;
            }			
			reg.PC += 1;
			
			yield break;
		}
		
		public static IEnumerable<bool> RLA(Memory mem, Registers reg)
		{
			Debug.Log(0, "RLA");
			int val = (reg.A << 1) + ((reg.fC) ? 1 : 0);
			
			reg.fZ = false;
			reg.fN = false;
			reg.fH = false;
			reg.fC = val > 0xFF;
			
			reg.A = (byte)val;
			
			//Console.WriteLine("{0} {1}", val, reg.A);
			
			reg.PC += 1;
			
			yield break;			
		}
		
		public static IEnumerable<bool> DECREMENT_REG(Memory mem, Registers reg)
		{
			byte val = 0;
			switch(mem[reg.PC])
			{
				case 0x05:
					val = reg.B;
					reg.B -= 1;
					Debug.Log(0, "DEC B");
					break;
				case 0x15:
					val = reg.D;
					reg.D -= 1;
					Debug.Log(0, "DEC D");
					break;
				case 0x0D:
					val = reg.C;
					reg.C -= 1;
					Debug.Log(0, "DEC C");
					break;
				case 0x1D:
					val = reg.E;
					reg.E -= 1;
					Debug.Log(0, "DEC E");
					break;
				case 0x3D:
					val = reg.A;
					reg.A -= 1;
					Debug.Log(0, "DEC A");
					break;
			}
			reg.fZ = (val - 1) == 0;
			reg.fN = true;
			reg.fH = ZMath.CheckHalfBorrow(val, 1);
			
			reg.PC += 1;
			
			yield break;
		}
		
		public static IEnumerable<bool> LDI_HL_A(Memory mem, Registers reg)
		{
			mem[reg.HL++] = reg.A;
			yield return true;
		
			Debug.Log(0, "LDI (HL++), A");
			reg.PC += 1;
		
			yield break;
		}

        public static IEnumerable<bool> LDI_A_HL(Memory mem, Registers reg)
        {
            reg.A = mem[reg.HL++];
            yield return true;

            Debug.Log(0, "LDI A, (HL++)");
            reg.PC += 1;

            yield break;
        }

        public static IEnumerable<bool> INCREMENT_16_REG(Memory mem, Registers reg)
		{			
			switch(mem[reg.PC])
			{
				case 0x13:
					reg.DE += 1;
					Debug.Log(0, "INC DE");
					break;
				case 0x23:
					reg.HL += 1;
					Debug.Log(0, "INC HL");
					break;
			}
			yield return true;
			reg.PC += 1;
			
			yield break;
		}
		
		public static IEnumerable<bool> RETURN(Memory mem, Registers reg)
		{
			byte lower = mem[reg.SP++];
			yield return true;
			byte upper = mem[reg.SP++];
			yield return true;			
			reg.PC = (upper << 8) | lower;
			yield return true;
			
			Debug.Log(0, "RET");
			
			yield break;
		}
		
		public static IEnumerable<bool> COMPARE(Memory mem, Registers reg)
		{
			byte val = 0;
			switch(mem[reg.PC])
			{
                case 0xBE:
                    val = mem[reg.HL];
                    Debug.Log(0, "CP (HL)");
                    yield return true;
                    break;
				case 0xFE:
					val = mem[reg.PC + 1];
					Debug.Log(0, "CP d8");
                    reg.PC += 1;
                    yield return true;
					break;
			}
			reg.fZ = reg.A == val;
			reg.fN = true;
			reg.fH = !ZMath.CheckHalfBorrow(reg.A, val);
			reg.fC = reg.A < val;
			
			reg.PC += 1;
			
			yield break;
		}
		
		public static IEnumerable<bool> JR_R8(Memory mem, Registers reg)
		{
			int n = (sbyte)mem[reg.PC + 1];
			yield return true;
			
			reg.PC += 2 + n; // Account for length of JR instruction
			yield return true;
			
			Debug.Log(0, "JR r8");
			
			yield break;
		}
		
		public static IEnumerable<bool> LOAD_A_FFNN(Memory mem, Registers reg)
		{
			byte n = mem[reg.PC + 1];
			yield return true;
			reg.A = mem[0xFF00 + n];
			yield return true;
			
			Debug.Log(0, "LD A, (0xFF00 + d8)");
			reg.PC += 2;
			
			yield break;			
		}
		
		public static IEnumerable<bool> SUB(Memory mem, Registers reg)
		{
			byte val = reg.A;
			switch(mem[reg.PC])
			{
				case 0x90:
					val = reg.B;
					Debug.Log(0, "SUB B");
					break;
				default:
					throw new InvalidOperationException("Subtraction instruction has not been implemented yet!");
			}
			reg.fZ = reg.A - val == 0;
			reg.fN = true;
			reg.fH = !ZMath.CheckHalfBorrow(reg.A, val);
			reg.fC = reg.A > val;
			reg.A -= val;
			
			reg.PC += 1;
			yield break;
		}
		
        public static IEnumerable<bool> ADD(Memory mem, Registers reg)
        {
            byte val = 0;
            switch(mem[reg.PC])
            {
                case 0x86:
                    val = mem[reg.HL];
                    yield return true;
                    Debug.Log(0, "ADD A, (HL)");
                    break;
                case 0x87:
                    val = reg.A;
                    Debug.Log(0, "ADD A, A");
                    break;
            }
            int result = reg.A + val;

            reg.fZ = ((byte)result) == 0;
            reg.fN = false;
            reg.fH = ZMath.CheckHalfCarry(reg.A, val);
            reg.fC = result > 0xFF;

            reg.A = (byte)result;
            reg.PC += 1;
            yield break;
        }

        public static IEnumerable<bool> JUMP(Memory mem, Registers reg)
        {
            byte low = mem[reg.PC + 1];
            yield return true;
            byte high = mem[reg.PC + 2];
            yield return true;

            reg.PC = (high << 8) | low;
            yield return true;
            //Debug.Log(200, "JUMP {0:X4}", reg.PC);
            Console.WriteLine("JUMP {0:X4}", reg.PC);
            yield break;
        }

        public static IEnumerable<bool> DI(Memory mem, Registers reg)
        {
            reg.DisableInterrupts();

            reg.PC += 1;
            yield break;
        }

        public static IEnumerable<bool> EI(Memory mem, Registers reg)
        {
            reg.EnableInterrupts();

            reg.PC += 1;
            yield break;
        }

        public static IEnumerable<bool> DECREMENT_NN(Memory mem, Registers reg)
        {
            switch(mem[reg.PC])
            {
                case 0x0B:
                    reg.BC -= 1;
                    yield return true;
                    Debug.Log(0, "DEC BC");
                    break;
            }
            reg.PC += 1;
            yield break;
        }

        public static IEnumerable<bool> OR(Memory mem, Registers reg)
        {
            switch (mem[reg.PC])
            {
                case 0xB0:
                    reg.A |= reg.B;
                    Debug.Log(0, "OR B");
                    break;
                case 0xB1:
                    reg.A |= reg.C;
                    Debug.Log(0, "OR C");
                    break;
            }
            reg.fZ = reg.A == 0;
            reg.fN = false;
            reg.fH = false;
            reg.fC = false;

            reg.PC += 1;
            yield break;
        }

        public static IEnumerable<bool> COMPLEMENT(Memory mem, Registers reg)
        {
            reg.A = (byte)~reg.A;
            reg.PC += 1;

            reg.fN = true;
            reg.fH = true;

            Debug.Log(0, "CPL A");
            yield break;
        }

        public static IEnumerable<bool> AND(Memory mem, Registers reg)
        {
            switch(mem[reg.PC])
            {
                case 0xA1:
                    reg.A &= reg.C;
                    Debug.Log(0, "AND C");
                    break;
                case 0xE6:
                    reg.PC += 1;
                    yield return true;
                    reg.A &= mem[reg.PC];
                    Debug.Log(0, "AND d8");
                    break;
            }
            reg.fZ = reg.A == 0;
            reg.fN = false;
            reg.fH = true;
            reg.fC = false;

            reg.PC += 1;
            yield break;
        }

        public static IEnumerable<bool> RESTART(Memory mem, Registers reg)
        {
            reg.PC += 1;
            yield return true;

            mem[--reg.SP] = (byte)(reg.PC >> 8);
            mem[--reg.SP] = (byte)(reg.PC & 0xFF);
            yield return true;

            switch (mem[reg.PC - 1])
            {
                case 0xEF:
                    reg.PC = 0x28;
                    break;
            }
            yield return true;

            Debug.Log(50, "Restart from {0:X4}", reg.PC);
            yield break;
        }

        public static IEnumerable<bool> ADD_HL_NN(Memory mem, Registers reg)
        {
            int val = 0;
            switch(mem[reg.PC])
            {
                case 0x19:
                    val = reg.DE;
                    Debug.Log(0, "ADD HL, DE");
                    break;
            }
            yield return true;
            int result = val + reg.HL;

            // Reset flag N
            reg.fN = false;
            
            // Check for carry from bit 11
            reg.fH = ((val & 0xFFF) + (reg.HL & 0xFFF)) > 0xFFF;

            // Check for overflow
            reg.HL = 0xFFFF & result;
            reg.fC = result > reg.HL;

            reg.PC += 1;
            yield break;
        }
	}

    // -----------------------------------------------------------------------------------------------------------------------------------------------
    //  CB Prefix Commands
    // -----------------------------------------------------------------------------------------------------------------------------------------------

    static class PrefixCB
	{
		public static IEnumerable<bool> HandleCB(Memory mem, Registers reg)
		{
			Debug.Log(0, "CB-{0:X2}: ", mem[reg.PC+1]);
			switch(mem[reg.PC+1])
			{
				case 0x11:
					return RL(mem, reg);
                case 0x37:
                    return SWAP(mem, reg);
				case 0x7C:
					return BIT(mem, reg);
				default:
					throw new InvalidOperationException(String.Format("CB-{0:X2} has not been implemented yet!", mem[reg.PC+1]));
			}
		}
		
		private static IEnumerable<bool> BIT(Memory mem, Registers reg)
		{
			Debug.Log(0, "BIT ");
			int t = 0;
			switch(mem[reg.PC+1])
			{
				case 0x7C:
					Debug.Log(0, "7, H");
					t = reg.H & (1 << 7);
					break;
			}
			reg.fZ = (t&0xFF) == 0;
			reg.fN = false;
			reg.fH = true;
			
			yield break;
		}
		
		private static IEnumerable<bool> RL(Memory mem, Registers reg) // Rotate Left, store bit 7 in fC
		{
			Debug.Log(0, "RL ");
			int val = 0;
			switch(mem[reg.PC+1])
			{
				case 0x11:
					val = (reg.C << 1) + ((reg.fC) ? 1 : 0);
					reg.C = (byte)val;
					Debug.Log(0, "C");
					break;
			}
			reg.fZ = (val&0xFF) == 0;
			reg.fN = false;
			reg.fH = false;
			reg.fC = val > 0xFF;			
			
			yield break;
		}

        private static IEnumerable<bool> SWAP(Memory mem, Registers reg)
        {
            switch(mem[reg.PC + 1])
            {
                case 0x37:
                    int low = reg.A & 0x0F;
                    int high = reg.A & 0xF0;
                    low <<= 4;
                    high >>= 4;
                    reg.A = (byte)(low | high);
                    reg.fZ = reg.A == 0;
                    break;
            }
            reg.fN = false;
            reg.fH = false;
            reg.fC = false;

            yield break;
        }
	}
}