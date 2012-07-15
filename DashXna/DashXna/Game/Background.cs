using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Dash
{
    public class Background : DrawableGameComponent
    {
        public static int NumBackgrounds = 2;
        public static int NumSlices = 11;
        public static int ScreenW;
        public static int ScreenH;
        public static int SliceWidth;

        private List<Obstacle> slices;
        private List<Texture2D> textures;
        private SpriteBatch spriteBatch;

        public Background(Game game) : base(game)
        {
            var cs = game.Content;

            this.slices = new List<Obstacle>();
            this.textures = new List<Texture2D>();

            ScreenW = game.GraphicsDevice.PresentationParameters.BackBufferWidth;
            ScreenH = game.GraphicsDevice.PresentationParameters.BackBufferHeight;
            SliceWidth = (int)Math.Ceiling((double)ScreenW / (NumSlices - 1));
        }

        protected override void LoadContent()
        {            
            
            for (int i = 0; i < NumBackgrounds; i++)
            {
                textures.Add(Game.Content.Load<Texture2D>("BackgroundSlice" + i));
            }

            for (int i = 0; i < NumSlices; i++)
            {
                var bg = new Obstacle(Game.Content, textures);
                bg.offset = i * SliceWidth;
                slices.Add(bg);
            }

            spriteBatch = new SpriteBatch(GraphicsDevice);

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            foreach (Obstacle slice in slices)
                slice.Update();

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            foreach (Obstacle slice in slices)
            {
                slice.DrawUsing(spriteBatch);
            }
            spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}
