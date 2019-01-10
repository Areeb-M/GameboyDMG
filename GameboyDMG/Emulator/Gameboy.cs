using System;
using System.Diagnostics;

namespace Emulator
{
    public class GameBoy
    {
        private InterruptController interruptController;
        private Timer timer;
        private PPU ppu;
        private Registers registers;
        private Memory memory;
        private CPU cpu;
        private Clock clock;

        private Stopwatch stopwatch;

        public GameBoy(string romPath)
        {
            // Base Derivation
            Setup();
            memory = new Memory(romPath, registers, ppu);
            cpu = new CPU(interruptController, memory, registers);
            clock = new Clock(timer, cpu, ppu);
        }

        public GameBoy(string romPath, string bootROMPath)
        {
            Setup();
            memory = new Memory(romPath, bootROMPath, registers, ppu);
            cpu = new CPU(interruptController, memory, registers);
            clock = new Clock(timer, cpu, ppu);
        }

        private void Setup()
        {

            interruptController = new InterruptController();
            timer = new Timer(interruptController);
            ppu = new PPU(interruptController, lcd);
            registers = new Registers(timer.TimerRegisters, ppu.DisplayRegisters);

            stopwatch = new Stopwatch();
        }

        public void Tick()
        {
            clock.Tick();
        }

    }

}