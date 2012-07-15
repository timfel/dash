﻿using System;
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
            public Texture2D texture;
            public double offset;
            private int speed;
            private Obstacles controller;

            public Obstacle(Obstacles controller, Texture2D texture, int speed)
            {
                this.controller = controller;
                this.texture = texture;
                this.speed = speed;
                this.offset = controller.ScreenW;
            }

            public void Draw(SpriteBatch sb)
            {
                sb.Draw(texture, new Rectangle((int)offset, controller.ScreenH / 2, texture.Width, texture.Height), Color.White);
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
        private int ScreenH;
        private int ScreenW;

        public Obstacles(Game game) : base(game)
        {
            this.textures = new List<Texture2D>();
            this.obstacles = new List<Obstacle>();
            DrawOrder = 9;

            ScreenW = game.GraphicsDevice.PresentationParameters.BackBufferWidth;
            ScreenH = game.GraphicsDevice.PresentationParameters.BackBufferHeight;
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
            spriteBatch.Begin();
            foreach (var o in obstacles)
                o.Draw(spriteBatch);
            spriteBatch.End();
        }

        public override void Update(GameTime time)
        {
            if (time.TotalGameTime.Seconds < 10)
                this.UpdateStage1(time);
            else if (time.TotalGameTime.Seconds < 30)
                this.UpdateStage2(time);
            else if (time.TotalGameTime.Seconds < 70)
                this.UpdateStage3(time);
            else
                this.UpdateStage4(time);

            foreach (Obstacle o in obstacles.ToArray())
                o.Update(time);
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
                if ((last == null || last.offset < ScreenW + last.texture.Width * 2) && time.TotalGameTime.Milliseconds % moduloChance == 0)
                {
                    var t = RandomTexture(textureChoices, 0);
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