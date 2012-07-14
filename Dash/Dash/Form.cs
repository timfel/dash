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
