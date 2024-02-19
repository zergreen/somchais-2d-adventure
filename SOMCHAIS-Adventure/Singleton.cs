using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOMCHAIS_Adventure
{
    class Singleton
    {
        public const int SCREENWIDTH = 800;
        public const int SCREENHEIGHT = 600;

        public enum GameState
        {
            StartNewLife,
            GamePlaying,
            GameOver,
            GameWin
        }
        public GameState CurrentGameState;

        public KeyboardState PreviousKey, CurrentKey;
        private static Singleton instance;

        private Singleton() { }

        public static Singleton Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Singleton();
                }
                return instance;
            }
        }

        public float heroX, heroY;
        public Player player;

    }
}
