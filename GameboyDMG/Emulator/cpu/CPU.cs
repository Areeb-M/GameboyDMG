using System;
using System.Collections;
using System.Collections.Generic;

namespace Emulator
{
	public class CPU
	{
		Memory memory;
		Registers reg;
		InterruptController ic;
		
		private IEnumerator opcode;

		public Registers registers
		{
			get { return reg; }
		}
		
		public CPU(InterruptController interruptController, Memory mem, Registers registers)
		{			
			memory = mem;
			ic = interruptController;
			reg	= registers;
			
			opcode = DefaultFunc().GetEnumerator();	
		}
		
		private IEnumerable<bool> DefaultFunc()
		{
			// A Placeholder function to act as the default opcode
			yield break;
		}
		
		public void Tick()
		{
            if (reg.GetEnableInterruptsFlag() == 1)
            {
                ic.EnableInterrupts();
            }
            if (reg.GetDisableInterruptsFlag() == 1)
            {
                ic.DisableInterrupts();
            }
			if (!opcode.MoveNext())
			{
				if (OpcodeTable.ContainsKey(memory[reg.PC]))
				{
					opcode = OpcodeTable.Call(memory[reg.PC], memory, reg);
					// Fetch takes 1 cycle
					Debug.Log(0, "\n{0:X2} - ", reg);
				}
				else
				{
                    string errorMessage = String.Format("\nUnknown Opcode: {0:X2} at {1:X4} - {2}", memory[reg.PC], reg.PC, reg);
                    throw new UnknownOpcodeException(errorMessage);
				}
				
			}
		}
	}	
}