using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;

namespace SmallNet.Samples
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class PongGame : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        PrimitiveBatch primitveBatch;
        Camera2D camera;
        Vector2 screenSize;

        string JOIN_IP = SNetUtil.getLocalIp(); // CHANGE TO THE IP ADDRESS YOU WOULD LIKE TO JOIN
        bool IS_HOST = true;                    // SET TO FALSE IF YOU DO NOT WANT TO HOST A GAME
        string credential = "local_player";     // CHANGE TO YOUR NAME


        BaseHost<PongModel, PongHostModel> host = new BaseHost<PongModel, PongHostModel>();
        BaseClient<PongModel> client = new BaseClient<PongModel>();

        

        public PongGame()
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
            screenSize = new Vector2(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            camera = new Camera2D(screenSize);
            //graphics.PreferredBackBufferHeight = (int)screenSize.Y;
            //graphics.PreferredBackBufferWidth = (int)screenSize.X;
            
            //graphics.ApplyChanges();
            primitveBatch = new PrimitiveBatch(this.GraphicsDevice);
            primitveBatch.setCamera(camera.Translation);

            if (this.IS_HOST)
            {
                this.host.Debug = true;
                this.host.start();
            }

            client.Debug = true;
            client.connectTo(this.JOIN_IP, this.credential);

            camera.OriginSet(Vector2.Zero);
            camera.PositionSet(Vector2.Zero);
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
            client.shutdown();
            host.shutdown();

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
            host.update(gameTime);
            camera.Update(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            primitveBatch.setCamera(camera.Translation);
            client.ClientModel.draw(screenSize, primitveBatch);


            base.Draw(gameTime);
        }
    }
}
