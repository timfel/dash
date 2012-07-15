using System;
using System.Net;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using DashXna;

namespace Dash
{
    public class Obstacles : DrawableGameComponent
    {
        private class Obstacle
        {
            public Texture2D texture;
            public double offset;
            private int speed;
            private Obstacles controller;
            private bool passed = false;

            public Obstacle(Obstacles controller, Texture2D texture, int speed)
            {
                this.controller = controller;
                this.texture = texture;
                this.speed = speed;
                this.offset = controller.ScreenW;

                if (speed < 0)
                {
                    if (IsJumper) this.speed = 450;
                    else if (IsDucker) this.speed = 400;
                    else this.speed = 400;
                }
            }

            public void Draw(SpriteBatch sb)
            {
                sb.Draw(texture, new Rectangle((int)offset, 0, texture.Width, texture.Height), Color.White);
            }

            public void Update(GameTime time)
            {
                var player = controller.Player;
                if (!passed && offset < player.pos.X + player.Bounds.Width / 2)
                {
                    passed = true;
                    if (!player.isRunning() && (IsJumper && player.isJumping() || IsDucker && player.isDucking() || IsBroom && player.isFlying()))
                    {
                        if (IsBroom && player.isFlying())
                        {
                            player.Kill();
                        }

                        int pts = 10;

                        if (IsBroom) pts += 35;

                        player.Highscore.AddPoints(pts);
                    }
                    else
                    {
                        player.OnCollision();
                    }
                }
                offset -= speed * time.ElapsedGameTime.TotalSeconds;
                if (offset < -texture.Width)
                    this.controller.RemoveObstacle(this);
            }

            public bool IsJumper { get { return this.controller.textures.IndexOf(this.texture) < Jumpers.Count; } }
            public bool IsDucker
            {
                get
                {
                    var index = this.controller.textures.IndexOf(this.texture);
                    return index >= Jumpers.Count && index < Jumpers.Count + Duckers.Count;
                }
            }
            public bool IsBroom { get { return !this.IsJumper && !this.IsDucker; } }
        }

        private static List<string> Jumpers = new List<string> { "lions", "river", "rocks", "alligator" };
        private static List<string> Duckers = new List<string> { "tree", "airplane", "rocket" };
        private static List<string> Brooms = new List<string> { "bisons", "burning_loop" };
        private static int ObstacleSpeed = 300; // 300 pixels/s
        private static int JumpersSpeed = 400;

        private List<Texture2D> textures;
        private SpriteBatch spriteBatch;
        private SpriteFont font;
        private List<Obstacle> obstacles;
        private int ScreenH;
        private int ScreenW;
        private Game1 game;
        private GameplayComponent gameplay;
        private SoundManager sounds;

        public Player Player { get { return gameplay.player; } }

        private double timeEllapsed = 0;

        public Obstacles(Game1 game)
            : base(game)
        {
            this.game = game;
            this.textures = new List<Texture2D>();
            this.obstacles = new List<Obstacle>();
            DrawOrder = 11;

            ScreenW = game.GraphicsDevice.PresentationParameters.BackBufferWidth;
            ScreenH = game.GraphicsDevice.PresentationParameters.BackBufferHeight;
        }

        public override void Initialize()
        {
            gameplay = Game.Services.GetService(typeof(GameplayComponent)) as GameplayComponent;
            sounds = Game.Services.GetService(typeof(SoundManager)) as SoundManager;

            base.Initialize();
        }

        protected override void OnEnabledChanged(object sender, EventArgs args)
        {
            if (Enabled)
                timeEllapsed = 0;

            base.OnEnabledChanged(sender, args);
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Game.Content.Load<SpriteFont>("Default");

            foreach (var s in Jumpers)
                textures.Add(Game.Content.Load<Texture2D>(s));
            foreach (var s in Duckers)
                textures.Add(Game.Content.Load<Texture2D>(s));
            foreach (var s in Brooms)
                textures.Add(Game.Content.Load<Texture2D>(s));

            base.LoadContent();
        }

        public override void Draw(GameTime time)
        {
            spriteBatch.Begin();
            foreach (var o in obstacles)
                o.Draw(spriteBatch);

            if (timeEllapsed < 6)
            {
                spriteBatch.DrawString(font, "Press the Left Arrow to jump,\nthe Right Arrow to duck,\nand both to become a broom.",
                    new Vector2(300, 150), Color.Black);
            }

            spriteBatch.End();
        }

        public override void Update(GameTime time)
        {
            if (timeEllapsed < 6)
                this.UpdateTutorial(time);
            else if (time.TotalGameTime.Seconds < 30)
                this.UpdateStage1(time);
            else if (time.TotalGameTime.Seconds < 60)
                this.UpdateStage2(time);
            else if (time.TotalGameTime.Seconds < 100)
                this.UpdateStage3(time);
            else
                this.UpdateStage4(time);

            foreach (Obstacle o in obstacles.ToArray())
                o.Update(time);

            timeEllapsed += time.ElapsedGameTime.TotalSeconds;
        }

        private void UpdateTutorial(GameTime time)
        {
        }

        private void UpdateStage1(GameTime time)
        {
            UpdateStageX(time, 1, 14, Jumpers.Count);
        }

        private void UpdateStage2(GameTime time)
        {
            UpdateStageX(time, 2, 14, Jumpers.Count + Duckers.Count);
        }

        private void UpdateStage3(GameTime time)
        {
            UpdateStageX(time, 2, 6, Jumpers.Count + Duckers.Count + Brooms.Count);
        }

        private void UpdateStage4(GameTime time)
        {
            UpdateStageX(time, 3, 6, Jumpers.Count + Duckers.Count + Brooms.Count);
        }

        private void UpdateStageX(GameTime time, int maxCount, int moduloChance, int textureChoices)
        {
            if (obstacles.Count < maxCount)
            {
                Obstacle last = null;
                if (obstacles.Count > 0)
                    last = obstacles[this.obstacles.Count - 1];

                if ((last == null || last.offset < last.texture.Width * 2) && time.TotalGameTime.Milliseconds % moduloChance == 0)
                {
                    var t = RandomTexture(textureChoices, 0);
                    this.obstacles.Add(new Obstacle(this, t, -1));
                }
            }
        }

        private Texture2D RandomJumper()
        {
            return RandomTexture(Jumpers.Count, 0);
        }

        private Texture2D RandomDucker()
        {
            return RandomTexture(Duckers.Count, Jumpers.Count);
        }

        private Texture2D RandomBroom()
        {
            return RandomTexture(Brooms.Count, Jumpers.Count + Duckers.Count);
        }

        private Texture2D RandomTexture(int count, int offset)
        {
            return textures[new Random().Next(count) + offset];
        }

        private void RemoveObstacle(Obstacle obstacle)
        {
            obstacles.Remove(obstacle);
        }
    }
}
