using System;

namespace GameProject0
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new DungeonMasters())
                game.Run();
        }
    }
}
