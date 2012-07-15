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
using DashXna;


namespace Dash
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class GameplayComponent : Microsoft.Xna.Framework.DrawableGameComponent
    {
        SpriteBatch spriteBatch;
        public Player player;
        Dash.Background background;        
        Obstacles obstacles;     
        HighscoreDisplay highscores;        
        SoundManager sound;
        SpriteManager sprites;
   
        private bool _visible = true, _enabled = true;

        public new bool Visible
        {
            get { return _visible; }
            set
            {
                _visible = value;
                if (player == null) return;
                player.Visible = value;
                background.Visible = value;
                obstacles.Visible = value;
                highscores.Visible = value;
                sprites.Visible = value;
            }
        }

        public new bool Enabled
        {
            get { return _enabled; }
            set
            {
                _enabled = value;
                if (player == null) return;
                player.Enabled = value;
                background.Enabled = value;
                obstacles.Enabled = value;
                highscores.Enabled = value;
                sprites.Enabled = value;
            }
        }

        public GameplayComponent(Game game)
            : base(game)
        {
            game.Services.AddService(typeof(GameplayComponent), this);
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            sprites = new SpriteManager(Game);
            Game.Components.Add(sprites);

            player = new Player(Game, this);
            Game.Components.Add(player);

            this.background = new Background(Game);
            Game.Components.Add(background);

            this.obstacles = new Obstacles(Game as Game1);
            Game.Components.Add(obstacles);

            highscores = new HighscoreDisplay(Game);
            Game.Components.Add(highscores);

            spriteBatch = new SpriteBatch(Game.GraphicsDevice);

            this.sound = Game.Services.GetService(typeof(SoundManager)) as SoundManager;            

            Enabled = false;
            Visible = true;

            base.Initialize();
        }

        public void Restart()
        {
            Game.Components.Remove(sprites);
            Game.Components.Remove(player);
            Game.Components.Remove(background);
            Game.Components.Remove(obstacles);
            Game.Components.Remove(highscores);

            highscores = new HighscoreDisplay(Game);
            highscores.Enabled = true;
            Game.Components.Add(highscores);

            sprites = new SpriteManager(Game);
            Game.Components.Add(sprites);

            player = new Player(Game, this);                        
            player.Lives = 300;
            Game.Components.Add(player);

            this.background = new Background(Game);
            Game.Components.Add(background);

            this.obstacles = new Obstacles(Game as Game1);
            Game.Components.Add(obstacles);            
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {

            base.Update(gameTime);
        }
    }
}
