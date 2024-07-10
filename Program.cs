using System;

namespace OBB_CD_Comparison
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new Game1())
            {
                game.IsFixedTimeStep = false;
                game.Run();
            }
        }
    }
}
