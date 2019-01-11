using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Emulator;
using GameboyDMG.Graphics;
using System;

namespace GameboyDMG
{
    public class GameboyDMG : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Manager manager;

        int scale;
        Rectangle GameBoyScreen;
        SpriteFont defaultFont;
        
        
        public GameboyDMG(Manager manager)
        {
            this.manager = manager;
            graphics = new GraphicsDeviceManager(this);

            scale = 4;
            GameBoyScreen = new Rectangle(0, 0, 160 * scale, 144 * scale);

            graphics.PreferredBackBufferHeight = GameBoyScreen.Height;
            graphics.PreferredBackBufferWidth = GameBoyScreen.Width;

            Content.RootDirectory = "Content";
        }
        
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            base.Initialize();
            GraphicsCore.CreateScreenModel(graphics);
            //this.IsFixedTimeStep = false;
            this.TargetElapsedTime = TimeSpan.FromSeconds(1.0f / 144.0f);
            graphics.SynchronizeWithVerticalRetrace = true;
            manager.Start();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            defaultFont = Content.Load<SpriteFont>("fonts/DefaultFont");
            // TODO: use this.Content to load your game content here
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            manager.Stop();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }
        
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            System.Drawing.Bitmap render = manager.ReadScreen();
            Texture2D result = GraphicsCore.GetTexture2DFromBitmap(graphics.GraphicsDevice, render);

            spriteBatch.Begin();
        
            GraphicsCore.Draw3DScreen(spriteBatch, graphics, result);

            double framerate = (1 / gameTime.ElapsedGameTime.TotalSeconds);
            string fpsCounter = string.Format("FPS: {0:N0}", framerate);
            spriteBatch.DrawString(defaultFont, fpsCounter, new Vector2(0, 0), Color.Red);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
