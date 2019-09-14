using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootShapesUp
{
    public partial class GameRoot
    {
        Rectangle HealthBit;
        private void DrawGUI(int Stage, GameTime gameTime)
        {
            switch (_state)
            {
                case GameState.LevelOne:
                case GameState.LevelTwo:
                    spriteBatch.DrawString(GameRoot.Font, (String.Format("Level {0}     Exp {1}", PlayerShip.Level, PlayerShip.Exp)), new Vector2(ScreenSize.X / 40, ScreenSize.Y / 40), Color.White);
                    spriteBatch.DrawString(GameRoot.Font, String.Format("Health: "), new Vector2(ScreenSize.X / 40, (ScreenSize.Y / 40) * 2), Color.White);
                    spriteBatch.DrawString(GameRoot.Font, String.Format("Lives: {0}", PlayerShip.Instance.Lives), new Vector2(ScreenSize.X / 40, (ScreenSize.Y / 40) * 3), Color.White);
                    spriteBatch.DrawString(GameRoot.Font, (String.Format("Stage {0}", Stage)), new Vector2(ScreenSize.X - 200, 30), Color.White);

                    for (int i = 1; i <= PlayerShip.HealthMax; i++)
                    {
                        int StartofHealthBar = 9 * 24;
                        HealthBit = new Rectangle(StartofHealthBar + (24 * i), 60, 24, 24);
                        if (i <= PlayerShip.Health)
                        {
                            spriteBatch.Draw(texture, HealthBit, Color.GreenYellow);
                        }
                        else
                        {
                            spriteBatch.Draw(texture, HealthBit, Color.Black);
                        }
                        DrawBorder(HealthBit, 2, Color.Black);
                    }
                    break;
                    case GameState.GameOver:
                    spriteBatch.DrawString(GameRoot.Font, String.Format("Game Over!"), new Vector2(ScreenSize.X / 2, (ScreenSize.Y / 2)), Color.White);
                    break;
                   case GameState.BossLevel:
                    spriteBatch.DrawString(GameRoot.Font, (String.Format("Level {0}     Exp {1}", PlayerShip.Level, PlayerShip.Exp)), new Vector2(ScreenSize.X / 40, ScreenSize.Y / 40), Color.White);
                    spriteBatch.DrawString(GameRoot.Font, String.Format("Health: "), new Vector2(ScreenSize.X / 40, (ScreenSize.Y / 40) * 2), Color.White);
                    spriteBatch.DrawString(GameRoot.Font, String.Format("Lives: {0}", PlayerShip.Instance.Lives), new Vector2(ScreenSize.X / 40, (ScreenSize.Y / 40) * 3), Color.White);
                    spriteBatch.DrawString(GameRoot.Font, (String.Format("Boss Level")), new Vector2(ScreenSize.X - 200, 30), Color.White);

                    for (int i = 1; i <= PlayerShip.HealthMax; i++)
                    {
                        int StartofHealthBar = 9 * 24;
                        HealthBit = new Rectangle(StartofHealthBar + (24 * i), 60, 24, 24);
                        if (i <= PlayerShip.Health)
                        {
                            spriteBatch.Draw(texture, HealthBit, Color.GreenYellow);
                        }
                        else
                        {
                            spriteBatch.Draw(texture, HealthBit, Color.Black);
                        }
                        DrawBorder(HealthBit, 2, Color.Black);
                    }
                    break;
                    /*case 4:
                        spriteBatch.Draw(image, Position, null, Color.White, Orientation, Size / 2f, 1f, 0, 0);
                        break;*/
            }
        }
    }
}
