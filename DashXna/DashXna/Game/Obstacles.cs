using System;
using System.Net;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

namespace Dash
{
    public class Obstacles : DrawableGameComponent
    {
        private class Obstacle
        {
            private Texture2D texture;
            private double offset;
            private int speed;
            private Obstacles controller;

            public Obstacle(Obstacles controller, Texture2D texture, int speed)
            {
                this.controller = controller;
                this.texture = texture;
                this.speed = speed;
            }

            public void Draw(SpriteBatch sb)
            {
                sb.Draw(texture, new Rectangle((int)offset, 0, texture.Width, texture.Height), Color.White);
            }

            public void Update(GameTime time)
            {                
                offset -= speed * time.ElapsedGameTime.TotalSeconds;
                if (offset < -texture.Width)
                    this.controller.RemoveObstacle(this);
            }
        }

        private static List<string> Jumpers = new List<string> { "lions", "river", "rocks", "alligator" };
        private static List<string> Duckers = new List<string> { "tree", "airplane", "rocket" };
        private static List<string> Brooms = new List<string> { "bisons", "burning_loop" };
        private static int ObstacleSpeed = 300; // 300 pixels/s

        private List<Texture2D> textures;
        private SpriteBatch spriteBatch;
        private List<Obstacle> obstacles;

        public Obstacles(Game game) : base(game)
        {
            this.textures = new List<Texture2D>();
            this.obstacles = new List<Obstacle>();
            DrawOrder = 9;
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

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
            foreach (var o in obstacles)
                o.Draw(spriteBatch);
        }

        public override void Update(GameTime time)
        {
            if (time.ElapsedGameTime.Seconds > 10)
                this.UpdateStage1(time);
            else if (time.ElapsedGameTime.Seconds > 30)
                this.UpdateStage2(time);
            else if (time.ElapsedGameTime.Seconds > 70)
                this.UpdateStage3(time);
            else
                this.UpdateStage4(time);
        }

        private void UpdateStage1(GameTime time)
        {
            if (this.obstacles.Count < 1)
            {
                var t = RandomJumper();
                if (time.TotalGameTime.Milliseconds % 14 == 0)
                {
                    this.obstacles.Add(new Obstacle(this, t, ObstacleSpeed));
                }
            }
        }

        private void UpdateStage2(GameTime time)
        {
            if (this.obstacles.Count < 2)
            {
                var t = RandomTexture(Jumpers.Count + Duckers.Count, 0);
                if (time.TotalGameTime.Milliseconds % 14 == 0)
                {
                    this.obstacles.Add(new Obstacle(this, t, ObstacleSpeed));
                }
            }
        }

        private void UpdateStage3(GameTime time)
        {
            if (this.obstacles.Count < 2)
            {
                var t = RandomTexture(Jumpers.Count + Duckers.Count, 0);
                if (time.TotalGameTime.Milliseconds % 6 == 0)
                {
                    this.obstacles.Add(new Obstacle(this, t, ObstacleSpeed));
                }
            }
        }

        private void UpdateStage4(GameTime time)
        {
            if (this.obstacles.Count < 3)
            {
                var t = RandomTexture(Jumpers.Count + Duckers.Count, 0);
                if (time.TotalGameTime.Milliseconds % 6 == 0)
                {
                    this.obstacles.Add(new Obstacle(this, t, ObstacleSpeed));
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
