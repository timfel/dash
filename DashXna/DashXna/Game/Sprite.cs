using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Diagnostics;


namespace Dash
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Sprite
    {
        protected Texture2D texture;
        protected Color taint = Color.White;     

        protected Vector2 Size
        {
            get;
            set;
        }

        public Vector2 Position
        {
            get;
            set;
        }

        public Sprite(Texture2D texture)
        {
            Size = new Vector2(texture.Width, texture.Height);
            this.texture = texture;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Position, taint);
        }

        public virtual void Update(GameTime gameTime)
        {
        }
    }

    public class AnimatedSprite : Sprite
    {
        int rows, cols;
        int numFrames;
        double currentFrame = 0;

        public int FPS
        {
            get;
            set;
        }

        public AnimatedSprite(Texture2D texture, Vector2 size, int fps) : base(texture)
        {
            Debug.Assert(size.X <= texture.Width && size.Y <= texture.Height);
            Debug.Assert(texture.Width % size.X == 0 && texture.Height % size.Y == 0);

            Size = size;
            FPS = fps;

            cols = texture.Width / (int)size.X;
            rows = texture.Height / (int)size.Y;
            numFrames = cols * rows;
        }

        public AnimatedSprite(Texture2D texture, Vector2 size)
            : this(texture, size, 23)
        {

        }

        Rectangle GetFrameSource(int frame)
        {
            float x = (frame % cols) * Size.X;
            float y = (float)Math.Floor(frame / cols) * Size.Y;
            return new Rectangle((int)x, (int)y, (int)Size.X, (int)Size.Y);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Rectangle src = GetFrameSource((int)currentFrame);
            spriteBatch.Draw(texture, Position, src, taint);
        }

        public override void Update(GameTime gameTime)
        {
            currentFrame += gameTime.ElapsedGameTime.TotalSeconds * (float)FPS;
            if (currentFrame > numFrames) currentFrame -= numFrames;
        }
    }
}
