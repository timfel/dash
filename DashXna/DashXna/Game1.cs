using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;
using Dash;

namespace DashXna
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Player player;
        IList<Form> forms = new List<Form>();
        Dash.Background background;
        double currentFormIndex = 0;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            // Frame rate is 30 fps by default for Windows Phone.
            TargetElapsedTime = TimeSpan.FromTicks(333333);

            // Extend battery life under lock.
            InactiveSleepTime = TimeSpan.FromSeconds(1);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            player = new Player();
            this.background = new Background(this);
            Components.Add(background);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            forms.Add(new Form(new Microsoft.Xna.Framework.Rectangle(0, 0, 300, 342), Content.Load<Texture2D>("Dash1")));
            forms.Add(new Form(new Microsoft.Xna.Framework.Rectangle(0, 0, 300, 342), Content.Load<Texture2D>("Dash")));
            forms.Add(new Form(new Microsoft.Xna.Framework.Rectangle(0, 0, 300, 341), Content.Load<Texture2D>("Dash2")));

            player.pos.Y = 450;            
            this.background.StartMoving();

            base.LoadContent();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            currentFormIndex = (currentFormIndex + 10 * gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0) % forms.Count;            

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            var form = forms[(int)(currentFormIndex)];
            spriteBatch.Begin();

            this.background.SlicesDo((BackgroundSlice slice) =>
            {
                spriteBatch.Draw(slice.sprite, new Microsoft.Xna.Framework.Rectangle(slice.offset, 0, Dash.Background.SliceWidth, Dash.Background.ScreenH), Color.White);
            });

            spriteBatch.Draw(form.texture, new Microsoft.Xna.Framework.Rectangle((int)player.pos.X, (int)player.pos.Y - form.bounds.Height, form.bounds.Width, form.bounds.Height), Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
