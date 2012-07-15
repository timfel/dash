using System;
using System.Net;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Dash
{
    public class Player : DrawableGameComponent
    {
        public Vector2 pos;
        IList<Form> forms = new List<Form>();
        double currentFormIndex = 0;
        SpriteBatch spriteBatch;

        public Player(Game game) : base(game)
        {
            DrawOrder = 10;
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);

            forms.Add(new Form(new Microsoft.Xna.Framework.Rectangle(0, 0, 300, 342), Game.Content.Load<Texture2D>("Dash1")));
            forms.Add(new Form(new Microsoft.Xna.Framework.Rectangle(0, 0, 300, 342), Game.Content.Load<Texture2D>("Dash")));
            forms.Add(new Form(new Microsoft.Xna.Framework.Rectangle(0, 0, 300, 341), Game.Content.Load<Texture2D>("Dash2")));

            pos.Y = 450; 
            
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            currentFormIndex = (currentFormIndex + 10 * gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0) % forms.Count;
            
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            var form = forms[(int)(currentFormIndex)];
            spriteBatch.Begin();

            spriteBatch.Draw(form.texture, new Microsoft.Xna.Framework.Rectangle((int)pos.X, (int)pos.Y - form.bounds.Height, form.bounds.Width, form.bounds.Height), Color.White);

            spriteBatch.End();
            
            base.Draw(gameTime);
        }
    }
}
