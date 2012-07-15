using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Xml.Linq;
using Microsoft.Xna.Framework.Input.Touch;


namespace Dash
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class IntroScreen : Microsoft.Xna.Framework.DrawableGameComponent
    {
        SpriteBatch spriteBatch;
        SpriteFont font;
        Texture2D background;
        string story;
        Vector2 textSize;
        double textOffset;
        double opacity = 1;

        public IntroScreen(Game game)
            : base(game)
        {
            DrawOrder = 15;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            font = Game.Content.Load<SpriteFont>("Default");
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            background = Game.Content.Load<Texture2D>("StoryBackground");

            XDocument doc = XDocument.Load(TitleContainer.OpenStream("Content\\Intro-Story.xml"));

            story = doc.Descendants("story").First().Element("text").Value;
            textSize = font.MeasureString(story);
            textOffset = 500;

            base.LoadContent();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            textOffset -= gameTime.ElapsedGameTime.TotalSeconds * 40;

            if (TouchPanel.GetState().Any())
                textOffset = -5000;
            
            if (textOffset < -textSize.Y)
            {
                if (opacity > 0)
                {
                    opacity -= gameTime.ElapsedGameTime.TotalSeconds * 0.5f;
                }
                else
                {
                    this.Enabled = false;
                    this.Visible = false;
                    (Game.Services.GetService(typeof(GameplayComponent)) as GameplayComponent).Enabled = true;
                }
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            var transform = Matrix.Identity;

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default,
                RasterizerState.CullNone, null, transform);

            spriteBatch.Draw(background, new Vector2(0, 0), new Color(1, 1, 1, (float)opacity));

            spriteBatch.DrawString(font, "The Story (Tap to skip)", new Vector2(40, (float)textOffset - 10), Color.Gray);
            spriteBatch.DrawString(font, story, new Vector2(40, (int)(10 + textOffset)), Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
