﻿#region Using Statements
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
using SmallNet;

using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
#endregion

namespace SmallNet.Samples.BasicMove
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class BasicGame : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont font;
        PrimitiveBatch prim;
        Camera2D camera;


        string ip = "192.168.1.124";
        string userName = "chris";
        bool isHost = true;


        BaseHost<BasicClientModel, BasicHostModel> host;
        BaseClient<BasicClientModel> client;
        
        public BasicGame()
            : base()
        {

           
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

           
            camera = new Camera2D(new Vector2(800, 600));
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {

            prim = new PrimitiveBatch(GraphicsDevice);


            if (isHost) //we are running the host threads!!!
            {
                host = new BaseHost<BasicClientModel, BasicHostModel>();
                host.start();
            }


            //we need to connect to an ip address. 
            client = new BaseClient<BasicClientModel>();
            client.connectTo(ip, userName);


            this.IsFixedTimeStep = true;
            this.TargetElapsedTime = TimeSpan.FromMilliseconds(20);

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
            font = Content.Load<SpriteFont>("font");
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
 
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

            client.update(gameTime);
            if (host != null)
                host.update(gameTime);

            prim.setCamera(camera.Translation);
            camera.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            client.ClientModel.draw(prim, spriteBatch, font);

            base.Draw(gameTime);
        }
    }
}
