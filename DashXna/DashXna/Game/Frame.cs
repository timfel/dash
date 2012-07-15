using System;
using System.Net;
using Microsoft.Xna.Framework.Graphics;

namespace Dash
{
    public class Frame
    {
        public Microsoft.Xna.Framework.Rectangle bounds;
        public Texture2D texture;

        public int offsetX;
        public int offsetY;

        public Frame(Microsoft.Xna.Framework.Rectangle bounds, Texture2D texture)
        {
            this.bounds = bounds;
            this.texture = texture;
        }

        public Frame Copy()
        {
            return Copy(offsetX, offsetY);
        }

        public Frame Copy(int offsetX, int offsetY)
        {
            var frame = new Frame(this.bounds, this.texture);
            frame.offsetX = offsetX;
            frame.offsetY = offsetY;

            return frame;
        }
    }
}
