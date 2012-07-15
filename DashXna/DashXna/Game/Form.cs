using System;
using System.Net;
using Microsoft.Xna.Framework.Graphics;

namespace Dash
{
    public class Form
    {
        public Microsoft.Xna.Framework.Rectangle bounds;
        public Texture2D texture;

        public Form(Microsoft.Xna.Framework.Rectangle bounds, Texture2D texture)
        {
            this.bounds = bounds;
            this.texture = texture;
        }
    }
}
