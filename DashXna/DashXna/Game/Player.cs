using System;
using System.Net;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input.Touch;

namespace Dash
{
    public class Player : DrawableGameComponent
    {
        public Vector2 pos;

        double frameIndex = 0;

        SpriteBatch spriteBatch;

        IDictionary<String, Frame> sprites = new Dictionary<String, Frame>();

        IList<Frame> defaultAnimation = new List<Frame>();
        IList<Frame> jumpAnimation = new List<Frame>();
        IList<Frame> duckAnimation = new List<Frame>();
        IList<Frame> flyAnimation = new List<Frame>();

        IList<Frame> animation;

        public Player(Game game) : base(game)
        {
            DrawOrder = 10;

            animation = defaultAnimation;
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);

            sprites.Add("dash", new Frame(new Microsoft.Xna.Framework.Rectangle(0, 0, 200, 228), Game.Content.Load<Texture2D>("Dash")));
            sprites.Add("dash1", new Frame(new Microsoft.Xna.Framework.Rectangle(0, 0, 200, 228), Game.Content.Load<Texture2D>("Dash1")));
            sprites.Add("dash2", new Frame(new Microsoft.Xna.Framework.Rectangle(0, 0, 200, 227), Game.Content.Load<Texture2D>("Dash2")));

            sprites.Add("dash-ducked", new Frame(new Microsoft.Xna.Framework.Rectangle(0, 0, 251, 153), Game.Content.Load<Texture2D>("Dash-Ducked-Sliding")));

            sprites.Add("broom", new Frame(new Microsoft.Xna.Framework.Rectangle(0, 0, 238, 88), Game.Content.Load<Texture2D>("Besen")));

            sprites.Add("buttons", new Frame(new Microsoft.Xna.Framework.Rectangle(0, 0, 800, 480), Game.Content.Load<Texture2D>("Buttons")));

            defaultAnimation.Add(sprites["dash1"].Copy());
            defaultAnimation.Add(sprites["dash"].Copy());
            defaultAnimation.Add(sprites["dash2"].Copy());

            jumpAnimation.Add(sprites["dash2"].Copy());
            jumpAnimation.Add(sprites["dash2"].Copy());
            jumpAnimation.Add(sprites["dash2"].Copy(0, -100));
            jumpAnimation.Add(sprites["dash-ducked"].Copy(0, -100));
            jumpAnimation.Add(sprites["dash-ducked"].Copy(0, -100));
            jumpAnimation.Add(sprites["dash-ducked"].Copy(0, -100));
            jumpAnimation.Add(sprites["dash-ducked"].Copy(0, -100));

            duckAnimation.Add(sprites["dash-ducked"].Copy());
            duckAnimation.Add(sprites["dash-ducked"].Copy());
            duckAnimation.Add(sprites["dash-ducked"].Copy());
            duckAnimation.Add(sprites["dash-ducked"].Copy());

            flyAnimation.Add(sprites["broom"].Copy(0, -200));
            flyAnimation.Add(sprites["broom"].Copy(0, -200));
            flyAnimation.Add(sprites["broom"].Copy(0, -200));
            flyAnimation.Add(sprites["broom"].Copy(0, -200));
            flyAnimation.Add(sprites["broom"].Copy(0, -200));
            flyAnimation.Add(sprites["broom"].Copy(0, -200));
            flyAnimation.Add(sprites["broom"].Copy(0, -200));
            flyAnimation.Add(sprites["broom"].Copy(0, -200));

            pos.X = 50;
            pos.Y = 370;
            
            base.LoadContent();
        }

        public void Run()
        {
            animation = defaultAnimation;
            frameIndex = 0;
        }

        public void Jump()
        {
            animation = jumpAnimation;
            frameIndex = 0;
        }

        public void Duck()
        {
            animation = duckAnimation;
            frameIndex = 0;
        }

        public void Fly()
        {
            animation = flyAnimation;
            frameIndex = 0;
        }

        protected void CheckInput()
        {
            // Process touch events
            TouchCollection touches = TouchPanel.GetState();
            Func<TouchLocation, Boolean> isLeftButton = (TouchLocation tl) =>
            {
                return tl.State == TouchLocationState.Pressed && tl.Position.X >= 0 && tl.Position.X <= 100 && tl.Position.Y >= 380;
            };
            Func<TouchLocation, Boolean> isRightButton = (TouchLocation tl) =>
            {
                return tl.State == TouchLocationState.Pressed && tl.Position.X >= 700 && tl.Position.X <= 800 && tl.Position.Y >= 380;
            };
            bool isLeft = false;
            bool isRight = false;

            foreach (TouchLocation tl in touches)
            {
                if (isLeftButton(tl))
                {
                    isLeft = true;
                    break;
                }
            }

            foreach (TouchLocation tl in touches)
            {
                if (isRightButton(tl))
                {
                    isRight = true;
                    break;
                }
            }

            if (isLeft && isRight)
            {
                Fly();
            }
            else if (isLeft)
            {
                Jump();
            }
            else if (isRight)
            {
                Duck();
            }
        }

        public override void Update(GameTime gameTime)
        {
            var oldFrameIndex = frameIndex;
            frameIndex = (frameIndex + 10 * gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0);

            CheckInput();

            if (frameIndex > animation.Count)
            {
                if (animation != defaultAnimation)
                {
                    Run();
                }
                frameIndex = 0;
            }
            
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            var frame = animation[(int)(frameIndex)];
            spriteBatch.Begin();

            spriteBatch.Draw(frame.texture, new Microsoft.Xna.Framework.Rectangle(
                (int)pos.X + frame.offsetX,
                (int)pos.Y + frame.offsetY - frame.bounds.Height,
                frame.bounds.Width, frame.bounds.Height), Color.White);

            var buttons = sprites["buttons"];

            spriteBatch.Draw(buttons.texture, buttons.bounds, Color.White);

            spriteBatch.End();
            
            base.Draw(gameTime);
        }
    }
}
