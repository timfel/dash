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


namespace Dash
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class HighscoreDisplay : Microsoft.Xna.Framework.DrawableGameComponent
    {
        SpriteBatch spriteBatch;
        SpriteFont normalFont, specialFont;

        double timePoints = 0;

        Stack<ScoreAnimation> scoreAnimations = new Stack<ScoreAnimation>();

        /// <summary>
        /// Number of points that are added to the highscore automatically each second
        /// </summary>
        public int PointsPerSecond
        {
            get;
            set;
        }

        public int Score
        {
            get;
            private set;
        }

        public HighscoreDisplay(Game game)
            : base(game)
        {
            game.Services.AddService(typeof(HighscoreDisplay), this);
            DrawOrder = 14;

            Score = 0;
            PointsPerSecond = 1;
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            normalFont = Game.Content.Load<SpriteFont>("HighscoreFont");
            specialFont = Game.Content.Load<SpriteFont>("HighscoreFont2");
            
            base.LoadContent();
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {            
            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            timePoints += gameTime.ElapsedGameTime.TotalSeconds;
            if (timePoints > 1)
            {
                Score += 1;
                timePoints -= 1;
            }

            UpdateScoreAnimations(gameTime);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            spriteBatch.DrawString(normalFont, "Score: " + Score, new Vector2(10, 10), Color.Black);

            foreach (var anim in scoreAnimations) 
                anim.Draw(gameTime, spriteBatch, specialFont);

            spriteBatch.End();
            
            base.Draw(gameTime);
        }

        private void UpdateScoreAnimations(GameTime gameTime)
        {
            foreach (var anim in scoreAnimations)
            {
                anim.Update(gameTime);
            }

            while (scoreAnimations.Any() && scoreAnimations.Peek().Finished)
                scoreAnimations.Pop();
        }

        public void AddPoints(int pts)
        {
            var offset = new Vector2(10 + normalFont.MeasureString("Score: " + Score).X + 10, 10 + scoreAnimations.Count() * 20);
            scoreAnimations.Push(new ScoreAnimation(pts, 1.0f, () => Score += pts, offset));
        }

        internal class ScoreAnimation
        {
            int points;
            Vector2 posOffset;
            double duration;
            double timeLeft;
            Action callback;            

            public bool Finished
            {
                get;
                private set;
            }

            public float Opacity
            {
                get;
                private set;
            }

            public ScoreAnimation(int pts, float duration, Action callback, Vector2 posOffset)
            {
                points = pts;
                this.duration = duration;
                this.callback = callback;
                this.posOffset = posOffset;
                timeLeft = duration;
                Finished = false;
            }

            public void Update(GameTime gameTime)
            {
                timeLeft -= gameTime.ElapsedGameTime.TotalSeconds;
                if (!Finished && timeLeft <= 0)
                {
                    callback();
                    Finished = true;
                }
            }

            /// <summary>
            /// Returns the time scale factor t which is 1.0 at the beginning of the animation and 0.0 at the end
            /// </summary>
            private float t
            {
                get
                {
                    return Math.Max(0, Math.Min(1, (float)(timeLeft / duration)));
                }
            }

            private Vector2 GetPosition(GameTime gameTime)
            {
                float x = 0;
                float y = 10 * t * t;
                return new Vector2(x,y) + posOffset;
            }

            private float GetOpacity(GameTime gameTime)
            {
                return t > 0.3 ? (1 - t * t) : t;
            }

            private Color GetColor(GameTime gameTime)
            {                
                return new Color(0, 0, 0, GetOpacity(gameTime));
            }

            public void Draw(GameTime gameTime, SpriteBatch spriteBatch, SpriteFont font)
            {
                var str = points < 0 ? ""+points : "+" + points;
                Color color = GetColor(gameTime);
                Opacity = GetOpacity(gameTime);
                spriteBatch.DrawString(font, str, GetPosition(gameTime), GetColor(gameTime));
            }
        }
    }

    
}