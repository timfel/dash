using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Windows;

namespace Dash
{
    class Background
    {
        public static int NumBackgrounds = 2;
        public static int NumSlices = 10;
        public static int ScreenW = (int)Application.Current.RootVisual.RenderSize.Height;
        public static int ScreenH = (int)Application.Current.RootVisual.RenderSize.Width;
        public static int SliceWidth = (int)Math.Ceiling((double)ScreenW / NumSlices);

        private List<BackgroundSlice> slices;
        private List<Texture2D> textures;

        public Background(ContentManager cs)
        {
            this.slices = new List<BackgroundSlice>();
            this.textures = new List<Texture2D>();
            for (int i = 0; i < NumBackgrounds; i++)
            {
                textures.Add(cs.Load<Texture2D>("BackgroundSlice" + i));
            }
            for (int i = 0; i < NumSlices; i++)
            {
                var bg = new BackgroundSlice(cs, textures.ElementAt(new Random().Next(NumBackgrounds)));
                bg.offset = i * SliceWidth;
                slices.Add(bg);
            }
        }

        public void SlicesDo(Action<BackgroundSlice> func) {
            foreach(var slice in slices)
            {
                func.Invoke(slice);
            }
        }
    }
}
