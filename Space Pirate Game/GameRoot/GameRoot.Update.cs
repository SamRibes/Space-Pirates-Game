using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootShapesUp
{
    public partial class GameRoot
    {
        bool start = false;
        int checkyet = 50;
        void UpdateMouseState(GameTime gameTime)
        {
            // update mouse variables
            MouseState mouse_state = Mouse.GetState();
            mx = mouse_state.X;
            my = mouse_state.Y;
            prev_mpressed = mpressed;
            mpressed = mouse_state.LeftButton == ButtonState.Pressed;
            update_buttons();
        }

        void UpdateMainMenu(GameTime gameTime)
        {
            // Respond to user input for menu selections , etc
            UpdateMouseState(gameTime);
        }

        void UpdateLevelOne(GameTime gameTime)
        {
            if(start == false)
            {
                PlayerShip.Level = 1;
                start = true;
            }
            // Respond to user actions in the game.
            // Update enemies for Level 1
            // Handle collisions
            EntityManager.Update();
            EnemySpawner.Update(1);
            if (PlayerShip.Instance.IsOver == true)
                _state = GameState.GameOver;
            if (PlayerShip.Level >= 5)
            {
                EntityManager.enemies.ForEach(x => x.IsExpired = true);
                EntityManager.enemybullets.ForEach(x => x.IsExpired = true);
                EntityManager.playerbullets.ForEach(x => x.IsExpired = true);
                EntityManager.bossentities.ForEach(x => x.IsExpired = true);
                EntityManager.pickups.ForEach(x => x.IsExpired = true);
                EnemySpawner.Reset();
                titleisdone = false;
                title_screen_length = 50;
                PlayerShip.Health = PlayerShip.HealthMax;
                start = false;
                _state = GameState.LevelTwo;
            }
        }

        void UpdateLevelTwo(GameTime gameTime)
        {
            EntityManager.Update();
            EnemySpawner.Update(2);
            if (PlayerShip.Instance.IsOver == true)
                _state = GameState.GameOver;
            if (PlayerShip.Level == 10)
            {
                EntityManager.enemies.ForEach(x => x.IsExpired = true);
                EntityManager.enemybullets.ForEach(x => x.IsExpired = true);
                EntityManager.playerbullets.ForEach(x => x.IsExpired = true);
                EntityManager.bossentities.ForEach(x => x.IsExpired = true);
                EntityManager.pickups.ForEach(x => x.IsExpired = true);
                EnemySpawner.Reset();
                titleisdone = false;
                title_screen_length = 50;
                PlayerShip.Health = PlayerShip.HealthMax;
                _state = GameState.BossLevel;
            }
        }

        void UpdateBossLevel(GameTime gameTime)
        {
            EntityManager.Update();
            EnemySpawner.Update(3);
            if (start == false)
            {
                start = true;
                PlayerShip.Instance.Position = new Vector2(100, ScreenSize.Y / 2);
            }
            if (PlayerShip.Instance.IsOver == true)
            {
                EntityManager.enemybullets.ForEach(x => x.IsExpired = true);
                EntityManager.playerbullets.ForEach(x => x.IsExpired = true);
                EntityManager.bossentities.ForEach(x => x.IsExpired = true);
                start = false;
                _state = GameState.GameOver;
            }
            checkyet--;
            if(checkyet <= 0)
            if (EntityManager.bossentities.Count == 1)
            {
                EntityManager.enemybullets.ForEach(x => x.IsExpired = true);
                EntityManager.playerbullets.ForEach(x => x.IsExpired = true);
                EntityManager.bossentities.ForEach(x => x.IsExpired = true);
                start = false;
                    checkyet = 50;
                _state = GameState.GameComplete;
            }

            if (PlayerShip.Instance.IsDead)
                PlayerShip.Instance.Position = new Vector2(100, ScreenSize.Y / 2);
        }

        void UpdateGameComplete(GameTime gameTime)
        {
            UpdateMouseState(gameTime);
        }

        void UpdateEndOfGame(GameTime gameTime)
        {
            UpdateMouseState(gameTime);
        }
    }
}
