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
        bool titleisdone = false;
        int title_screen_length = 50;
        void DrawMainMenu(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Draw(Title_Screen, Vector2.Zero, Color.White);

            for (int i = 0; i < NUMBER_OF_BUTTONS; i++)
                spriteBatch.Draw(button_texture[i], button_rectangle[i], button_color[i]);

            // draw the custom mouse cursor
            spriteBatch.Draw(GameRoot.Pointer, Input.MousePosition, Color.White);
            base.Draw(gameTime);
        }

        void DrawLevelOne(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            if (titleisdone == false && title_screen_length > 0)
            {
                spriteBatch.Draw(Title_Screen_1, Vector2.Zero, Color.White);
                title_screen_length--;
                if (title_screen_length <= 0)
                    titleisdone = true;
            }
            else
            {
                spriteBatch.Draw(Background1, Vector2.Zero, Color.White);
                // Draw user interface
                spriteBatch.Draw(Background1, Vector2.Zero, Color.White);
                foreach (var entity in EntityManager.entities)
                    entity.Draw(spriteBatch);
                DrawGUI(1, gameTime);
                // draw the custom mouse cursor
                spriteBatch.Draw(GameRoot.Crosshair, new Vector2(Input.MousePosition.X - 13, Input.MousePosition.Y + 13), Color.White);
                base.Draw(gameTime);
            }
        }

        void DrawLevelTwo(GameTime gameTime)
        {
            if (titleisdone == false && title_screen_length > 0)
            {
                spriteBatch.Draw(Title_Screen_2, Vector2.Zero, Color.White);
                title_screen_length--;
                if (title_screen_length <= 0)
                    titleisdone = true;
            }
            else
            {
                GraphicsDevice.Clear(Color.Black);
                spriteBatch.Draw(Background2, Vector2.Zero, Color.White);
                foreach (var entity in EntityManager.entities)
                    entity.Draw(spriteBatch);
                DrawGUI(2, gameTime);
                // draw the custom mouse cursor
                spriteBatch.Draw(GameRoot.Crosshair, new Vector2(Input.MousePosition.X - 13, Input.MousePosition.Y + 13), Color.White);
                base.Draw(gameTime);
            }
        }

        void DrawBossLevel(GameTime gameTime)
        {
            if (titleisdone == false && title_screen_length > 0)
            {
                spriteBatch.Draw(Title_Screen_3, Vector2.Zero, Color.White);
                title_screen_length--;
                if (title_screen_length <= 0)
                    titleisdone = true;
            }
            else
            {
                GraphicsDevice.Clear(Color.Black);
                // Draw user interface
                spriteBatch.Draw(Background3, Vector2.Zero, Color.White);
                foreach (var entity in EntityManager.entities)
                    entity.Draw(spriteBatch);
                DrawGUI(3, gameTime);
                // draw the custom mouse cursor
                spriteBatch.Draw(GameRoot.Crosshair, new Vector2(Input.MousePosition.X - 13, Input.MousePosition.Y + 13), Color.White);
                base.Draw(gameTime);
            }
        }

        void DrawGameOver(GameTime gameTime)
        {
           GraphicsDevice.Clear(Color.Black);
            spriteBatch.Draw(Game_Over, Vector2.Zero, Color.White);

            for (int i = 0; i < NUMBER_OF_BUTTONS; i++)
                spriteBatch.Draw(button_texture[i], button_rectangle[i], button_color[i]);

            // draw the custom mouse cursor
            spriteBatch.Draw(GameRoot.Pointer, Input.MousePosition, Color.White);
            base.Draw(gameTime);
        }

        void DrawGameComplete(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Draw(Game_Complete, Vector2.Zero, Color.White);

            for (int i = 0; i < NUMBER_OF_BUTTONS; i++)
                spriteBatch.Draw(button_texture[i], button_rectangle[i], button_color[i]);

            // draw the custom mouse cursor
            spriteBatch.Draw(GameRoot.Pointer, Input.MousePosition, Color.White);
            base.Draw(gameTime);
        }




        private void DrawBorder(Rectangle rectangleToDraw, int thicknessOfBorder, Color borderColor)
        {
            // Draw top line
            spriteBatch.Draw(pixel, new Rectangle(rectangleToDraw.X, rectangleToDraw.Y, rectangleToDraw.Width, thicknessOfBorder), borderColor);

            // Draw left line
            spriteBatch.Draw(pixel, new Rectangle(rectangleToDraw.X, rectangleToDraw.Y, thicknessOfBorder, rectangleToDraw.Height), borderColor);

            // Draw right line
            spriteBatch.Draw(pixel, new Rectangle((rectangleToDraw.X + rectangleToDraw.Width - thicknessOfBorder),
                                            rectangleToDraw.Y,
                                            thicknessOfBorder,
                                            rectangleToDraw.Height), borderColor);
            // Draw bottom line
            spriteBatch.Draw(pixel, new Rectangle(rectangleToDraw.X,
                                            rectangleToDraw.Y + rectangleToDraw.Height - thicknessOfBorder,
                                            rectangleToDraw.Width,
                                            thicknessOfBorder), borderColor);
        }
    }
}
