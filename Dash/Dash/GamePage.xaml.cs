using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;

namespace Dash
{
    public partial class GamePage : PhoneApplicationPage
    {
        ContentManager contentManager;
        GameTimer timer;
        SpriteBatch spriteBatch;

        Texture2D dash;
        Player player;
        Dash.Background background;

        public GamePage()
        {
            InitializeComponent();

            // Content-Manager von der Anwendung holen
            contentManager = (Application.Current as App).Content;

            // Timer für diese Seite erstellen
            timer = new GameTimer();
            timer.UpdateInterval = TimeSpan.FromTicks(333333);
            timer.Update += OnUpdate;
            timer.Draw += OnDraw;

            player = new Player();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Freigabemodus des Grafikgeräts so einstellen, das es sich beim XNA-Rendering einschaltet
            SharedGraphicsDeviceManager.Current.GraphicsDevice.SetSharingMode(true);

            // Erstellen Sie einen neuen SpriteBatch, der zum Zeichnen von Texturen verwendet werden kann.
            spriteBatch = new SpriteBatch(SharedGraphicsDeviceManager.Current.GraphicsDevice);

            // TODO: Verwenden Sie this.content, um Ihren Spiel-Content hier zu laden

            this.dash = contentManager.Load<Texture2D>("Dash");
            this.background = new Background(contentManager);

            // Timer starten
            timer.Start();

            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            // Timer stoppen
            timer.Stop();

            // Freigabemodus des Grafikgeräts so einstellen, das es sich beim XNA-Rendering ausschaltet
            SharedGraphicsDeviceManager.Current.GraphicsDevice.SetSharingMode(false);

            base.OnNavigatedFrom(e);
        }

        /// <summary>
        /// Ermöglicht der Seite die Ausführung der Logik, wie zum Beispiel Aktualisierung der Welt,
        /// Überprüfung auf Kollisionen, Erfassung von Eingaben und Abspielen von Ton.
        /// </summary>
        private void OnUpdate(object sender, GameTimerEventArgs e)
        {
            // TODO: Fügen Sie Ihre Aktualisierungslogik hier hinzu

            // Process touch events
            TouchCollection touchCollection = TouchPanel.GetState();
            foreach (TouchLocation tl in touchCollection)
            {
                if ((tl.State == TouchLocationState.Pressed)
                        || (tl.State == TouchLocationState.Moved))
                {
                    player.pos.X += 10;
                    if (player.pos.X > 700) player.pos.X = 0;
                }
            }

        }

        /// <summary>
        /// Ermöglicht der Seite, selbst zu zeichnen.
        /// </summary>
        private void OnDraw(object sender, GameTimerEventArgs e)
        {
            SharedGraphicsDeviceManager.Current.GraphicsDevice.Clear(Color.White);

            spriteBatch.Begin();            
            this.background.SlicesDo((BackgroundSlice slice) => {
                spriteBatch.Draw(slice.sprite, new Microsoft.Xna.Framework.Rectangle(slice.offset, 0, Dash.Background.SliceWidth, Dash.Background.ScreenH), Color.White);
            });
            spriteBatch.Draw(dash, new Microsoft.Xna.Framework.Rectangle((int)player.pos.X, (int)player.pos.Y, 300, 342), Color.White);
            spriteBatch.End();
        }
    }
}
