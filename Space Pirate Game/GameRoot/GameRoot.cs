using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShootShapesUp
{
    public partial class GameRoot : Game
    {
        public GameRoot()
        {
            Instance = this;
            graphics = new GraphicsDeviceManager(this)
            {
                GraphicsProfile = GraphicsProfile.HiDef
            };
            Content.RootDirectory = @"Content";

            this.graphics.IsFullScreen = true;
            this.graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            this.graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
        }

        protected override void Initialize()
        {
            base.Initialize();

            InitializeMenu();
            IsMouseVisible = false;
            background_color = Color.Black;

            EntityManager.Add(PlayerShip.Instance);

            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(GameRoot.Music);
            MediaPlayer.Volume = 0.1f;
            SoundEffect.MasterVolume = 0.1f;
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            //Player sprite
            Player = Content.Load<Texture2D>("Art/Player");
            //Enemy sprites
            Basic = Content.Load<Texture2D>("Art/Enemies/Basic_Pirate");
            TwinShot = Content.Load<Texture2D>("Art/Enemies/TwinShot");
            Asteroid = Content.Load<Texture2D>("Art/Enemies/Asteroid");
            MissileEnemy = Content.Load<Texture2D>("Art/Enemies/Missile_Enemy");
            //Boss sprites
            Boss_Body = Content.Load<Texture2D>("Art/Boss/Boss");
            Boss_Turret_1 = Content.Load<Texture2D>("Art/Boss/Boss_Turret_1");
            Boss_Turret_2 = Content.Load<Texture2D>("Art/Boss/Boss_Turret_2");
            Dead_Boss = Content.Load<Texture2D>("Art/Boss/Dead_Boss");
            //Bullet sprites
            PlayerBullet = Content.Load<Texture2D>("Art/Bullets/Player_Bullet");
            BasicBullet = Content.Load<Texture2D>("Art/Bullets/Basic_Bullet");
            TwinBullet = Content.Load<Texture2D>("Art/Bullets/Twin_Bullet");
            Missile = Content.Load<Texture2D>("Art/Bullets/Missile");
            //Pickups
            Coin1 = Content.Load<Texture2D>("Art/Pickups/coin-1");
            Coin2 = Content.Load<Texture2D>("Art/Pickups/coin-2");
            Coin3 = Content.Load<Texture2D>("Art/Pickups/coin-3");
            Coin4 = Content.Load<Texture2D>("Art/Pickups/coin-4");
            Heart = Content.Load<Texture2D>("Art/Pickups/Heart");

            //Other
            Pointer = Content.Load<Texture2D>("Art/Pointer");
            Crosshair = Content.Load<Texture2D>("Art/crosshair");

            //Backgrounds
            Background1 = Content.Load<Texture2D>("Art/Backgrounds/Level1_Background");
            Background2 = Content.Load<Texture2D>("Art/Backgrounds/Level2_Background");
            Background3 = Content.Load<Texture2D>("Art/Backgrounds/Level3_Background");
            Title_Screen = Content.Load<Texture2D>("Art/Backgrounds/Title_Screen");
            Title_Screen_1 = Content.Load<Texture2D>("Art/Backgrounds/Title_Screen_1");
            Title_Screen_2 = Content.Load<Texture2D>("Art/Backgrounds/Title_Screen_2");
            Title_Screen_3 = Content.Load<Texture2D>("Art/Backgrounds/Title_Screen_3");
            Game_Complete = Content.Load<Texture2D>("Art/Backgrounds/Game_Complete");
            Game_Over = Content.Load<Texture2D>("Art/Backgrounds/Game_Over");

            //Asteroid = new SpriteSheet(Content.Load<Texture2D>("Art/asteroid_sprite_sheet"), 10f, 50, 50, 32);
            texture = new Texture2D(graphics.GraphicsDevice, 1, 1);
            texture.SetData(new Color[] { Color.Green });
            pixel = new Texture2D(graphics.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            pixel.SetData(new[] { Color.White });

            Font = Content.Load<SpriteFont>("Font/Font");

            Music = Content.Load<Song>("Sound/Music");

            button_texture[PLAY_BUTTON_INDEX] =
                Content.Load<Texture2D>("Art/Buttons/Start");
            button_texture[EXIT_BUTTON_INDEX] =
                Content.Load<Texture2D>("Art/Buttons/Exit");

            // These linq expressions are just a fancy way loading all sounds of each category into an array.
            explosions = Enumerable.Range(1, 8).Select(x => Content.Load<SoundEffect>("Sound/explosion-0" + x)).ToArray();
            shots = Enumerable.Range(1, 4).Select(x => Content.Load<SoundEffect>("Sound/shoot-0" + x)).ToArray();
            spawns = Enumerable.Range(1, 8).Select(x => Content.Load<SoundEffect>("Sound/spawn-0" + x)).ToArray();
        }

        protected override void Update(GameTime gameTime)
        {
            GameTime = gameTime;
            Input.Update();

            // Allows the game to exit
            if (Input.WasButtonPressed(Buttons.Back) || Input.WasKeyPressed(Keys.Escape))
                this.Exit();

            if (Input.WasKeyPressed(Keys.T))
                PlayerShip.Level ++;

            switch (_state)
            {
                case GameState.MainMenu:
                    UpdateMainMenu(gameTime);
                    break;
                case GameState.LevelOne:
                    UpdateLevelOne(gameTime);
                    break;
                case GameState.LevelTwo:
                    UpdateLevelTwo(gameTime);
                    break;
                case GameState.BossLevel:
                    UpdateBossLevel(gameTime);
                    break;
                case GameState.GameComplete:
                    UpdateGameComplete(gameTime);
                    break;
                case GameState.GameOver:
                    UpdateEndOfGame(gameTime);
                    break;
            }

            // get elapsed frame time in seconds
            frame_time = gameTime.ElapsedGameTime.Milliseconds / 1000.0;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            spriteBatch.Begin(SpriteSortMode.Deferred);
            switch (_state)
            {
                case GameState.MainMenu:
                    DrawMainMenu(gameTime);
                    break;
                case GameState.LevelOne:
                    DrawLevelOne(gameTime);
                    break;
                case GameState.LevelTwo:
                    DrawLevelTwo(gameTime);
                    break;
                case GameState.BossLevel:
                    DrawBossLevel(gameTime);
                    break;
                case GameState.GameComplete:
                    DrawGameComplete(gameTime);
                    break;
                case GameState.GameOver:
                    DrawGameOver(gameTime);
                    break;
            }
            spriteBatch.End();
        }
    }
}
