using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace GameProject0.Screens
{
    public class MainMenuScreen : MenuScreen
    {
        public MainMenuScreen() : base("Dungeon Masters")
        {
            TitleColor = Color.Black;

            var play = new MenuEntry("Play Game");
            // var settings = new MenuEntry("Settings");
            var exit = new MenuEntry("Exit");

            play.Selected += ClickPlay;
            exit.Selected += ClickExit;

            MenuEntries.Add(play);
            // MenuEntries.Add(settings);
            MenuEntries.Add(exit);
        }


        private void ClickExit(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.Game.Exit();
        }

        private void ClickPlay(object sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, true, e.PlayerIndex, new GameplayScreen());
        }
    }
}
