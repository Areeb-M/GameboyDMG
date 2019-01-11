using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GameboyDMG.Graphics
{
    class GraphicsCore
    {

        static VertexPositionTexture[] screenVertices;
        static BasicEffect screenEffect;

        public static void CreateScreenModel(GraphicsDeviceManager g)
        {
            screenVertices = new VertexPositionTexture[6];

            screenVertices[0].Position = new Vector3(-20, -20, 0);
            screenVertices[1].Position = new Vector3(-20, 20, 0);
            screenVertices[2].Position = new Vector3(20, -20, 0);
            screenVertices[3].Position = screenVertices[1].Position;
            screenVertices[4].Position = new Vector3(20, 20, 0);
            screenVertices[5].Position = screenVertices[2].Position;

            screenVertices[0].TextureCoordinate = new Vector2(1, 0);
            screenVertices[1].TextureCoordinate = new Vector2(1, 1);
            screenVertices[2].TextureCoordinate = new Vector2(0, 0);
            screenVertices[3].TextureCoordinate = screenVertices[1].TextureCoordinate;
            screenVertices[4].TextureCoordinate = new Vector2(0, 1);
            screenVertices[5].TextureCoordinate = screenVertices[2].TextureCoordinate;

            screenEffect = new BasicEffect(g.GraphicsDevice);
        }

        public static void Draw3DScreen(SpriteBatch s, GraphicsDeviceManager g, Texture2D tex)
        {
            // The assignment of effect.View and effect.Projection
            // are nearly identical to the code in the Model drawing code.
            var cameraPosition = new Vector3(0, 35, 50);
            var cameraLookAtVector = Vector3.Zero;
            var cameraUpVector = Vector3.UnitZ;

            screenEffect.View = Matrix.CreateLookAt(
                cameraPosition, cameraLookAtVector, cameraUpVector);

            float aspectRatio =
                g.PreferredBackBufferWidth / (float)g.PreferredBackBufferHeight;
            float fieldOfView = MathHelper.PiOver4;
            float nearClipPlane = 1;
            float farClipPlane = 200;

            screenEffect.Projection = Matrix.CreatePerspectiveFieldOfView(
                fieldOfView, aspectRatio, nearClipPlane, farClipPlane);

            screenEffect.TextureEnabled = true;
            screenEffect.Texture = tex;

            foreach (var pass in screenEffect.CurrentTechnique.Passes)
            {
                pass.Apply();

                g.GraphicsDevice.DrawUserPrimitives(
                    // We’ll be rendering two trinalges
                    PrimitiveType.TriangleList,
                    // The array of verts that we want to render
                    screenVertices,
                    // The offset, which is 0 since we want to start 
                    // at the beginning of the floorVerts array
                    0,
                    // The number of triangles to draw
                    2);
            }
        }


        public static Texture2D GetTexture2DFromBitmap(GraphicsDevice device, System.Drawing.Bitmap bitmap)
        {
            Texture2D tex = new Texture2D(device, bitmap.Width, bitmap.Height);

            BitmapData data = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, bitmap.PixelFormat);

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
