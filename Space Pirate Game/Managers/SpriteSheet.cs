using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ShootShapesUp
{
    public class SpriteSheet
    {
        private Texture2D texture;
        private Vector2 position;
        private float rotation;
        private Vector2 scale;
        private Color color;
        private float timer;
        private float interval;
        private int currentFrame;
        private int lastFrame;
        private int spriteWidth;
        private int spriteHeight;
        private Rectangle sourceRect;

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public float Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }

        public Vector2 Scale
        {
            get { return scale; }
            set { scale = value; }
        }

        public Color Color
        {
            get { return color; }
            set { color = value; }
        }

        public SpriteSheet(Texture2D spriteSheet, float interval, int spriteWidth, int spriteHeight, int frameCount)
        {
            this.texture = spriteSheet;
            this.position = new Vector2(50.0f, 50.0f);
            this.rotation = 55f;
            this.scale = new Vector2(0.5f, 0.5f);
            this.color = Color.White;
            this.timer = 0.0f;
            this.interval = interval;
            this.currentFrame = 1;
            this.lastFrame = frameCount;
            this.spriteWidth = spriteWidth;
            this.spriteHeight = spriteHeight;
        }

        public void Update(GameTime gameTime)
        {
            // Increase the timer by the number of milliseconds
            // since update was last called
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            // Check that timer exceeds the chosen interval
            if (timer > interval)
            {
                // If true, move on to the next frame
                currentFrame++;
                // Then reset the timer
                timer = 0f;
            }
            // If we are on the last frame, reset back to the one before
            // the first frame (because currentframe++ is called next
            // so the next frame will be 1!)
            if (currentFrame == lastFrame)
            {
                currentFrame = 0;
            }
            // Prepare the rectangle that defines the part of
            // the image to draw
            sourceRect =
            new Rectangle(currentFrame * spriteWidth, 0,
            spriteWidth, spriteHeight);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Using an overloaded parameter for Draw() that
            // has greater functionality
            spriteBatch.Draw(texture, position, sourceRect, color, rotation, new Vector2(spriteWidth / 2, spriteHeight / 2), scale, SpriteEffects.None, 0f);
        }
    }
}
