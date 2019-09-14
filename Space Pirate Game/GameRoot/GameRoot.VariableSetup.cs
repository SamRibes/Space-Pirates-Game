using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootShapesUp
{
    partial class GameRoot
    {
        public GameState _state;
        public enum GameState
        {
            MainMenu,
            LevelOne,
            LevelTwo,
            BossLevel,
            GameComplete,
            GameOver,
        }

        public enum BState
        {
            HOVER,
            UP,
            JUST_RELEASED,
            DOWN
        }

        public const int NUMBER_OF_BUTTONS = 2,
            PLAY_BUTTON_INDEX = 0, EXIT_BUTTON_INDEX = 1,
            BUTTON_HEIGHT = 190, BUTTON_WIDTH = 352;
        public Color background_color;
        public Color[] button_color = new Color[NUMBER_OF_BUTTONS];
        public Rectangle[] button_rectangle = new Rectangle[NUMBER_OF_BUTTONS];
        public BState[] button_state = new BState[NUMBER_OF_BUTTONS];
        public Texture2D[] button_texture = new Texture2D[NUMBER_OF_BUTTONS];
        public double[] button_timer = new double[NUMBER_OF_BUTTONS];
        //mouse pressed and mouse just pressed
        public bool mpressed, prev_mpressed = false;
        //mouse location in window
        public int mx, my;
        public double frame_time;

        public static GameRoot Instance { get; private set; }
        public static Viewport Viewport { get { return Instance.GraphicsDevice.Viewport; } }
        public static Vector2 ScreenSize { get { return new Vector2(Viewport.Width, Viewport.Height); } }
        public static GameTime GameTime { get; private set; }
        public static Texture2D Player { get; private set; }
        public static Texture2D Basic { get; private set; }
        public static Texture2D Boss_Body { get; private set; }
        public static Texture2D Dead_Boss { get; private set; }
        public static Texture2D Boss_Turret_1 { get; private set; }
        public static Texture2D Boss_Turret_2 { get; private set; }
        public static Texture2D TwinShot { get; private set; }
        public static Texture2D PlayerBullet { get; private set; }
        public static Texture2D BasicBullet { get; private set; }
        public static Texture2D TwinBullet { get; private set; }
        public static Texture2D Missile { get; private set; }
        public static Texture2D Pointer { get; private set; }
        public static Texture2D Crosshair { get; private set; }
        public static Texture2D Asteroid { get; private set; }
        public static Texture2D MissileEnemy { get; private set; }
        public static Texture2D Coin1 { get; private set; }
        public static Texture2D Coin2 { get; private set; }
        public static Texture2D Coin3 { get; private set; }
        public static Texture2D Coin4 { get; private set; }
        public static Texture2D Heart { get; private set; }
        public static Texture2D Background1 { get; private set; }
        public static Texture2D Background2 { get; private set; }
        public static Texture2D Background3 { get; private set; }
        public static Texture2D Title_Screen { get; private set; }
        public static Texture2D Title_Screen_1 { get; private set; }
        public static Texture2D Title_Screen_2 { get; private set; }
        public static Texture2D Title_Screen_3 { get; private set; }
        public static Texture2D Game_Complete { get; private set; }
        public static Texture2D Game_Over { get; private set; }

        Texture2D texture;
        Texture2D pixel;
        //public static SpriteSheet Asteroid { get; private set; }

        public static SpriteFont Font { get; private set; }

        public static Song Music { get; private set; }

        private static readonly Random rand = new Random();

        private static SoundEffect[] explosions;
        public static SoundEffect Explosion { get { return explosions[rand.Next(explosions.Length)]; } }

        private static SoundEffect[] shots;
        public static SoundEffect Shot { get { return shots[rand.Next(shots.Length)]; } }

        private static SoundEffect[] spawns;
        public static SoundEffect Spawn { get { return spawns[rand.Next(spawns.Length)]; } }

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
    }
}
