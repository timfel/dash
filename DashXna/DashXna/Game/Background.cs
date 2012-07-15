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
        public static int SliceWidth = (int)Math.Ceiling((double)ScreenW / (NumSlices - 1));

        private List<BackgroundSlice> slices;
        private List<Texture2D> textures;
        private GameTimer timer;

        public Background(Game game) : base(game)
        {
            var cs = game.Content;
            timer = new GameTimer();
            timer.UpdateInterval = TimeSpan.FromMilliseconds(40);

            this.slices = new List<BackgroundSlice>();
            this.textures = new List<Texture2D>();
                        
            ScreenW = game.GraphicsDevice.PresentationParameters.BackBufferWidth;
            ScreenH = game.GraphicsDevice.PresentationParameters.BackBufferHeight;
        }

        protected override void LoadContent()
        {            
            
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
