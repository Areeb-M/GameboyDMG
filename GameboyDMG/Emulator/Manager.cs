using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Emulator
{
    class Manager
    {
        private bool initialized;
        private bool paused;

        public bool IsGameboyInitialized { get { return initialized; } }
        public bool IsPaused { get { return paused;  } }


        private GameBoy gameboy;
        private Thread emulationThread;
        
        public Manager()
        {
            ThreadStart executionPoint = new ThreadStart(Run);
            emulationThread = new Thread(executionPoint);

            Debug.Log(100, "Initialized Manager!");
        }
        
        public void Start()
        {
            if (!initialized)
                throw new GameboyUninitializedException();

            emulationThread.Start();

        }

        private void Run()
        {
            while (true)
            {
                gameboy.Tick();
            }
        }

        // ===============================
        // Gameboy Controller Methods
        // ===============================

        
    }
}
