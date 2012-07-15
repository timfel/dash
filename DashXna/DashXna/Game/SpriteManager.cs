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
using System.Text.RegularExpressions;


namespace Dash
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class SpriteManager : Microsoft.Xna.Framework.DrawableGameComponent
    {
        List<List<Sprite>> layers = new List<List<Sprite>>(6);
        SpriteBatch spriteBatch;

        public enum Layer 
        {
            BackgroundFar = 0,
            BackgroundMiddle = 1,
            BackgroundNear = 2,
            Horizon = 3,
            Ground = 4,
            Front = 5
        }

        public SpriteManager(Game game)
            : base(game)
        {
            for (int i = 0; i < 6; i++) layers.Add(new List<Sprite>());
            DrawOrder = 13;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            foreach (var layer in layers)
                foreach (var sprite in layer)
                    sprite.Update(gameTime);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            foreach (var layer in layers)
                foreach (var sprite in layer)
                    sprite.Draw(spriteBatch);

            spriteBatch.End();
            
            base.Draw(gameTime);
        }
        

        public Sprite AddSprite(Sprite sprite, Layer layer)
        {
            layers[(int)layer].Add(sprite);
            return sprite;
        }

        public Sprite AddSprite(string name, Layer layer)
        {
            Sprite sprite = null;
            Match match = null;
            match = Regex.Match(name, @"(\d+)x(\d+).anim.(\d+)fps");

            if (match.Success)
            {
                int w = int.Parse(match.Groups[1].Value);
                int h = int.Parse(match.Groups[2].Value);
                int fps = int.Parse(match.Groups[3].Value);
                sprite = new AnimatedSprite(Game.Content.Load<Texture2D>(name), new Vector2(w, h), fps);
            }
            else
            {
                sprite = new Sprite(Game.Content.Load<Texture2D>(name));
            }

            layers[(int)layer].Add(sprite);
            return sprite;                   
        }

        public bool RemoveSprite(Sprite sprite)
        {
            foreach (var layer in layers)
                if (layer.Remove(sprite))
                    return true;

            return false;
        }
    }
}
