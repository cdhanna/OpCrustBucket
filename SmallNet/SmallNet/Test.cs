#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using System.Net.Sockets;
using System.IO;
using Open.Nat;

using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
#endregion

namespace SmallNet
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Test : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        //BaseHost<TestClientModel> host = new BaseHost<TestClientModel>();
        //BaseClient<TestClientModel> client = new BaseClient<TestClientModel>();

        DebugSession.DebugSession<TestClientModel, TestHostModel> debug;

        public Test()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            //this.TargetElapsedTime = TimeSpan.FromSeconds(1);

            //SNetUtil.discoverIps();

            //host.Debug = true;
            //host.start();

            //client.Debug = true;
            //client.connectTo(SNetUtil.getLocalIp(), "WillTest");


            //System.Threading.Thread.Sleep(100);
            //client.sendMessage("test", "1");

            //Console.WriteLine("init");

           debug = new DebugSession.DebugSession<TestClientModel, TestHostModel>();

            debug.start();

            Console.WriteLine("hello???");
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
            Console.WriteLine("load");
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            //client.shutdown();
            //host.shutdown();
            debug.stop();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            //client.update(gameTime);
            //host.update(gameTime);
           
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
