using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Dash
{
    public class BackgroundSlice
    {
        public static int NumBackgrounds = 2;
        
        public Texture2D sprite;
        public int offset;

        public BackgroundSlice(ContentManager cs, Texture2D sprite)
        {
            this.offset = 0;
            this.sprite = sprite;
        }
    }
}
