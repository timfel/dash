using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace Dash
{
    public class Background : DrawableGameComponent
    {
        private class Layer
        {
            private Texture2D texture;
            private double offset;
            private int speed;

            public double Offset { get { return offset; } }

            public Layer(Texture2D texture, int initialOffset, int speed)
            {
                this.texture = texture;
                this.offset = initialOffset;
                this.speed = speed;
            }

            public void Draw(SpriteBatch sb)
            {
                sb.Draw(texture, new Rectangle((int)offset, 0, texture.Width, ScreenH), Color.White);
                
            }

            public void Update(GameTime time)
            {                
                offset -= speed * time.ElapsedGameTime.TotalSeconds;
                if (offset < -texture.Width)
                    offset += texture.Width * 2;
            }
        }

        public static int NumLayers = 3;
        public static int NumSlices = 11;
        public static int ScreenW;
        public static int ScreenH;
        public static int SliceWidth;

        private List<Obstacle> slices;
        private List<Layer> layers;
        private List<Texture2D> textures;
        private SpriteBatch spriteBatch;

        public Background(Game game) : base(game)
        {
            var cs = game.Content;

            this.slices = new List<Obstacle>();
            this.layers = new List<Layer>();
            this.textures = new List<Texture2D>();

            ScreenW = game.GraphicsDevice.PresentationParameters.BackBufferWidth;
            ScreenH = game.GraphicsDevice.PresentationParameters.BackBufferHeight;
            SliceWidth = (int)Math.Ceiling((double)ScreenW / (NumSlices - 1));
        }

        protected override void LoadContent()
        {            
            
            for (int i = NumLayers - 1; i >= 0; i--)
            {
                var speed = (NumLayers + 1 - i) * 100;
                var t = Game.Content.Load<Texture2D>("BackgroundSlice" + i);
                layers.Add(new Layer(t, 0, speed));
                layers.Add(new Layer(t, t.Width, speed));
            }

            //for (int i = 0; i < numslices; i++)
            //{
            //    var bg = new obstacle(game.content, textures);
            //    bg.offset = i * slicewidth;
            //    slices.add(bg);
            //}

            spriteBatch = new SpriteBatch(GraphicsDevice);

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            foreach (Obstacle slice in slices)
                slice.Update();

            foreach (Layer l in layers)
                l.Update(gameTime);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            foreach (Layer l in layers)
                l.Draw(spriteBatch);

            //foreach (Obstacle slice in slices)
            //{
            //    slice.DrawUsing(spriteBatch);
            //}
            spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}
