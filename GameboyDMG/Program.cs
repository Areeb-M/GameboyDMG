using System;
using Emulator;

namespace GameboyDMG
{
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Manager manager = new Manager();
            manager.InitializeGameboy(args[0], args[1]);

            using (var game = new GameboyDMG(manager))
                game.Run();
        }
    }
}
