using System;
using System.Net;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace Dash
{
    public class Player : DrawableGameComponent
    {
        public Vector2 pos;

        double frameIndex = 0;
        private int lives = 3;

        SpriteBatch spriteBatch;

        IDictionary<String, Frame> sprites = new Dictionary<String, Frame>();

        IList<Frame> defaultAnimation = new List<Frame>();
        IList<Frame> jumpAnimation = new List<Frame>();
        IList<Frame> duckAnimation = new List<Frame>();
        IList<Frame> flyAnimation = new List<Frame>();

        IList<Frame> animation;

        HighscoreDisplay highscores;
        SoundManager sound;
        SoundEffectInstance gallopSound;

        GameplayComponent gameplay;

        public Player(Game game, GameplayComponent gameplay) : base(game)
        {
            this.gameplay = gameplay;

            DrawOrder = 10;

            animation = defaultAnimation;
        }

        public int Lives
        {
            get
            {
                return lives;
            }

            set
            {
                if (value > 0)
                {
                    lives = value;
                    Highscore.Lives = value;
                }
                else if (value <= 0)
                {
                    Highscore.Lives = 0;
                    // game over
                    gameplay.Enabled = false;
                    gallopSound.Stop();
                    MediaPlayer.Stop();
                }
            }
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
            
            gallopSound = sound.Loop("gallop");
            gallopSound.Volume = 1.0f;
            gallopSound.Pause();
            
            base.LoadContent();
        }

        public void Run()
        {
            if (animation == jumpAnimation)
                sound.Play("jumpland");

            animation = defaultAnimation;
            frameIndex = 0;                        
        }

        public bool isRunning()
        {
            return animation == defaultAnimation;
        }

        public void Jump()
        {
            animation = jumpAnimation;
            frameIndex = 0;
            sound.Play("jump");
        }

        public bool isJumping()
        {
            return animation == jumpAnimation;
        }

        public void Duck()
        {
            animation = duckAnimation;
            frameIndex = 0;
            sound.Play(Dash.SoundManager.Sound.slide);
        }

        public bool isDucking()
        {
            return animation == duckAnimation;
        }

        public void Fly()
        {
            animation = flyAnimation;
            frameIndex = 0;
        }

        public bool isFlying()
        {
            return animation == flyAnimation;
        }

        public Microsoft.Xna.Framework.Rectangle Bounds
        {
            get
            {
                return animation[(int) frameIndex].bounds;
            }
        }

        public HighscoreDisplay Highscore
        {
            get
            {
                return highscores;
            }
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

            bool running = isRunning();

            if (isLeft && isRight && running)
            {
                Fly();
            }
            else if (isLeft && running)
            {
                Jump();
            }
            else if (isRight && running)
            {
                Duck();
            }
        }

        public override void Initialize()
        {
            highscores = Game.Services.GetService(typeof(HighscoreDisplay)) as HighscoreDisplay;
            sound = Game.Services.GetService(typeof(SoundManager)) as SoundManager;

            Highscore.Lives = this.Lives;
            
            base.Initialize();
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

            if (gallopSound.State != SoundState.Playing && isRunning())
                gallopSound.Play();
            else if (!isRunning())
                gallopSound.Pause();
            
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
