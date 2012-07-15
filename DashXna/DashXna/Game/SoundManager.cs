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
    public class SoundManager : Microsoft.Xna.Framework.DrawableGameComponent
    {
        Dictionary<string, SoundEffect> sounds = new Dictionary<string, SoundEffect>();

        public enum Sound
        {
            gallop,
            jump,
            jumpland,
            slide,
            whoosh,
            impact
        }

        public SoundManager(Game game)
            : base(game)
        {
            game.Services.AddService(typeof(SoundManager), this);
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
            sounds.Add("gallop", Game.Content.Load<SoundEffect>("gallop"));
            sounds.Add("jump", Game.Content.Load<SoundEffect>("BounceYoFrankie"));
            sounds.Add("jumpland", Game.Content.Load<SoundEffect>("jumpland"));
            sounds.Add("slide", Game.Content.Load<SoundEffect>("slide"));
            sounds.Add("whoosh", Game.Content.Load<SoundEffect>("whoosh"));
            sounds.Add("impact", Game.Content.Load<SoundEffect>("body_impact_3_with_grunt_"));
            
            base.LoadContent();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here

            base.Update(gameTime);
        }

        public SoundEffectInstance Loop(string sound)
        {
            var inst = sounds[sound].CreateInstance();
            inst.IsLooped = true;
            inst.Play();            

            return inst;
        }

        public void Play(string sound)
        {
            sounds[sound].Play();            
        }

        public void Play(Sound sound)
        {
            sounds[sound.ToString()].Play();
        }
    }
}
