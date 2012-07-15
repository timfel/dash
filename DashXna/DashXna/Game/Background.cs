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

        private List<BackgroundSlice> slices;
        private List<Texture2D> textures;
        private GameTimer timer;
        private SpriteBatch spriteBatch;

        public Background(Game game) : base(game)
        {
            var cs = game.Content;
            timer = new GameTimer();
            timer.UpdateInterval = TimeSpan.FromMilliseconds(40);

            this.slices = new List<BackgroundSlice>();
            this.textures = new List<Texture2D>();                                   
        }

        public override void Initialize()
        {
            ScreenW = Game.GraphicsDevice.PresentationParameters.BackBufferWidth;
            ScreenH = Game.GraphicsDevice.PresentationParameters.BackBufferHeight;
            SliceWidth = (int)Math.Ceiling((double)ScreenW / (NumSlices - 1));
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);

            for (int i = 0; i < NumBackgrounds; i++)
            {
                textures.Add(Game.Content.Load<Texture2D>("BackgroundSlice" + i));
            }

            for (int i = 0; i < NumSlices; i++)
            {
                var bg = new BackgroundSlice(Game.Content, textures);
                bg.offset = i * SliceWidth;
                timer.Update += bg.Update;
                slices.Add(bg);
            }

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            foreach (var slice in slices)
                spriteBatch.Draw(slice.sprite, new Microsoft.Xna.Framework.Rectangle(slice.offset, 0, Dash.Background.SliceWidth, Dash.Background.ScreenH), Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        public void SlicesDo(Action<BackgroundSlice> func) {
            foreach(var slice in slices)
            {
                func.Invoke(slice);
            }
        }

        public void StartMoving()
        {
            timer.Start();
        }

        public void StopMoving()
        {
            timer.Stop();
        }
    }
}
