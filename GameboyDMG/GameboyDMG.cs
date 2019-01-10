using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Emulator;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace GameboyDMG
{
    public class GameboyDMG : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Manager manager;

        int scale;
        Rectangle GameBoyScreen;
        
        
        public GameboyDMG(Manager manager)
        {
            this.manager = manager;
            graphics = new GraphicsDeviceManager(this);

            scale = 3;
            GameBoyScreen = new Rectangle(0, 0, 160 * scale, 144 * scale);
            graphics.PreferredBackBufferHeight = GameBoyScreen.Height;
            graphics.PreferredBackBufferWidth = GameBoyScreen.Width;

            Content.RootDirectory = "Content";
        }
        
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            base.Initialize();
            manager.Start();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

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
            GraphicsDevice.Clear(Color.CornflowerBlue);
            System.Drawing.Bitmap render = manager.ReadScreen();
            Texture2D result = GetTexture2DFromBitmap(graphics.GraphicsDevice, render);

            spriteBatch.Begin();

            spriteBatch.Draw(result, GameBoyScreen, Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        public static Texture2D GetTexture2DFromBitmap(GraphicsDevice device, System.Drawing.Bitmap bitmap)
        {
            Texture2D tex = new Texture2D(device, bitmap.Width, bitmap.Height);

            BitmapData data = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, bitmap.PixelFormat);

            int bufferSize = data.Height * data.Stride;

            //create data buffer 
            byte[] bytes = new byte[bufferSize];

            // copy bitmap data into buffer
            Marshal.Copy(data.Scan0, bytes, 0, bytes.Length);

            // copy our buffer to the texture
            tex.SetData(bytes);

            // unlock the bitmap data
            bitmap.UnlockBits(data);

            return tex;
        }
    }
}
