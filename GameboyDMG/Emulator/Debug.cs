using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Emulator
{
    public static class Debug
    {
        private const int DEBUG_LEVEL = 50;

        private static void Write(string message, params object[] args)
        {
            Console.Write(message, args);            
        }

        public static void Log(int level, string message, params object[] args)
        {
            /* Ignore less important Debug Messages
             * 
             *  Levels:
             *  0 - Opcodes
             *  50 - Interrupts
             *  100 - General Info
            */
            if (level > DEBUG_LEVEL)
                Write(message, args);
        }

        public static void LogOpcode(int PC, byte opcode)
        {
            Log(0, "[{0:X4}]{1:X2}: ", PC, opcode);
        }
        /*
        public static void ShowConsole()
        {
            if (!AttachConsole(-1))
                AllocConsole();
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();

        [DllImport("kernel32", SetLastError = true)]
        private static extern bool AttachConsole(int dwProcessId);*/
    }
}
