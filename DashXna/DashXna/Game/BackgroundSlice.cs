using System;
using System.Net;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

namespace Dash
{
    public class BackgroundSlice
    {
        public static int NumBackgrounds = 2;

        public List<Texture2D> textures;
        public Texture2D sprite;
        public int offset;

        public BackgroundSlice(ContentManager cs, List<Texture2D> textures)
        {
            this.offset = 0;
            this.textures = textures;
            this.sprite = this.randomSprite();
        }

        public void Update()
        {
            this.offset -= 15;
            if (this.offset < -Background.SliceWidth)
            {
                this.sprite = this.randomSprite();
                this.offset = Background.ScreenW;
            }
        }

        public void DrawUsing(SpriteBatch sb)
        {
            sb.Draw(sprite, new Rectangle(offset, 0, Background.SliceWidth, Background.ScreenH), Color.White);
        }

        private Texture2D randomSprite()
        {
            return textures[new Random().Next(Background.NumBackgrounds)];
        }
    }
}
