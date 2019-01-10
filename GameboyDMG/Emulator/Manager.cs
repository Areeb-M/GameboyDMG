using System;
using System.Drawing;
using System.Threading;

namespace Emulator
{
    public class Manager
    {
        private bool initialized;
        private bool paused;
        private bool alive;

        public bool IsGameboyInitialized { get { return initialized; } }
        public bool IsPaused { get { return paused;  } }


        private GameBoy gameboy;
        private Thread emulationThread;
        
        public Manager()
        {
            ThreadStart executionPoint = new ThreadStart(Run);
            emulationThread = new Thread(executionPoint);

            Debug.Log(100, "Initialized Manager!\n");
        }
        
        public void InitializeGameboy(string romPath, string bootRomPath = "")
        {
            if (bootRomPath != "")
                gameboy = new GameBoy(romPath, bootRomPath);
            else
                gameboy = new GameBoy(romPath);

            Debug.Log(100, "Initialized GameBoy!\n");
            initialized = true;
            alive = true;
        }

        public void Start()
        {
            if (!initialized)
                throw new GameboyUninitializedException();

            emulationThread.Start();
            Debug.Log(100, "Beginning emulation.\n\n\n");
        }

        private void Run()
        {
            while (alive)
            {
                gameboy.Tick();
            }
            Debug.Log(100, "Stopped emulation.");
        }

        public void Stop()
        {
            alive = false;
            Debug.Log(100, "Stopping emulation.");
        }

        // ===============================
        // Gameboy Controller Methods
        // ===============================

        public Bitmap ReadScreen()
        {
            return gameboy.GetPPU().GetScreen();
        }
        
    }
}
