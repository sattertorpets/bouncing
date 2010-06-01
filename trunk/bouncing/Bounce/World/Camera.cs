using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bounce
{
    public class Camera
    {
        #region Fields

        private int screenHeight;
        private int screenWidth;
        private GraphicsDevice graphicsDevice;

        VertexDeclaration basicEffectVertexDeclaration;
        BasicEffect basicEffect;
        Matrix worldMatrix;
        Matrix viewMatrix;
        Matrix projectionMatrix;


        #endregion

        #region Properties

        public GraphicsDevice GrapicsDevice
        {
            get { return graphicsDevice; }
        }

        public VertexDeclaration BasicEffectVD
        {
            get { return basicEffectVertexDeclaration; }
        }

        public BasicEffect BaseEffect
        {
            get { return basicEffect; }
        }

        #endregion

        #region Initialization

        public Camera(int screenHeight, int screenWidth, GraphicsDevice graphics)
        {
            this.screenHeight = screenHeight;
            this.screenWidth = screenWidth;
            graphicsDevice = graphics;

            float tilt = MathHelper.ToRadians(0.0f);  // 0 degree angle
            // Use the world matrix to tilt the cube along x and y axes.
            worldMatrix = Matrix.CreateRotationX(tilt) *
                Matrix.CreateRotationY(tilt);

            viewMatrix = Matrix.CreateLookAt(new Vector3(0, 0, 1),
                new Vector3(0, 0, 0),
                Vector3.Up);

            projectionMatrix = Matrix.CreateOrthographicOffCenter(
                    0,
                    (float)screenWidth,
                    (float)screenHeight,
                    0,
                    1.0f, 1000.0f);

            basicEffect = new BasicEffect(graphicsDevice, null);
           /* basicEffect.Alpha = 1.0f;
            basicEffect.DiffuseColor = new Vector3(1.0f, 0.0f, 1.0f);
            basicEffect.SpecularColor = new Vector3(0.25f, 0.25f, 0.25f);
            basicEffect.SpecularPower = 5.0f;
            basicEffect.AmbientLightColor = new Vector3(0.75f, 0.75f, 0.75f);

            basicEffect.DirectionalLight0.Enabled = true;
            basicEffect.DirectionalLight0.DiffuseColor = Vector3.One;
            basicEffect.DirectionalLight0.Direction =
                Vector3.Normalize(new Vector3(1.0f, -1.0f, -1.0f));
            basicEffect.DirectionalLight0.SpecularColor = Vector3.One;

            basicEffect.DirectionalLight1.Enabled = true;
            basicEffect.DirectionalLight1.DiffuseColor =
                new Vector3(0.5f, 0.5f, 0.5f);
            basicEffect.DirectionalLight1.Direction =
                Vector3.Normalize(new Vector3(-1.0f, -1.0f, 1.0f));
            basicEffect.DirectionalLight1.SpecularColor =
                new Vector3(0.5f, 0.5f, 0.5f);

            basicEffect.LightingEnabled = true; */

            basicEffect.World = worldMatrix;
            basicEffect.View = viewMatrix;
            basicEffect.Projection = projectionMatrix;

            basicEffectVertexDeclaration = new VertexDeclaration(
                graphicsDevice, VertexPositionColor.VertexElements);

            graphicsDevice.VertexDeclaration =
                basicEffectVertexDeclaration;
        }

        #endregion
    }
}
