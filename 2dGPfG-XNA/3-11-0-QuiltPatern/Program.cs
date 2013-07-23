using System;

namespace QuiltPattern
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (Game2 game = new Game2())
            {
                game.Run();
            }
        }
    }
#endif
}

