using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Windows;
using Microsoft.Xna.Framework;

namespace Dash
{
    class Background
    {
        public static int NumBackgrounds = 2;
        public static int NumSlices = 11;
        public static int ScreenW = (int)Application.Current.Host.Content.ActualHeight;
        public static int ScreenH = (int)Application.Current.Host.Content.ActualWidth;
        public static int SliceWidth = (int)Math.Ceiling((double)ScreenW / (NumSlices - 1));

        private List<BackgroundSlice> slices;
        private List<Texture2D> textures;
        private GameTimer timer;

        public Background(ContentManager cs)
        {
            timer = new GameTimer();
            timer.UpdateInterval = TimeSpan.FromMilliseconds(40);

            this.slices = new List<BackgroundSlice>();
            this.textures = new List<Texture2D>();
            for (int i = 0; i < NumBackgrounds; i++)
            {
                textures.Add(cs.Load<Texture2D>("BackgroundSlice" + i));
            }
            for (int i = 0; i < NumSlices; i++)
            {
                var bg = new BackgroundSlice(cs, textures);
                bg.offset = i * SliceWidth;
                timer.Update += bg.Update;
                slices.Add(bg);
            }
        }

        public void SlicesDo(Action<BackgroundSlice> func) {
            foreach(var slice in slices)
            {
                func.Invoke(slice);
            }
        }

        internal void StartMoving()
        {
            timer.Start();
        }

        internal void StopMoving()
        {
            timer.Stop();
        }
    }
}
