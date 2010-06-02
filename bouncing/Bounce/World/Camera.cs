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

        private VertexDeclaration basicEffectVertexDeclaration;
        private BasicEffect basicEffect;
        private Matrix projectionMatrix;


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

        public Matrix ProjectionMatrix
        {
            get { return projectionMatrix; }
        }

        #endregion

        #region Initialization

        public Camera(int screenHeight, int screenWidth, GraphicsDevice graphics)
        {
            this.screenHeight = screenHeight;
            this.screenWidth = screenWidth;
            graphicsDevice = graphics;

            projectionMatrix = Matrix.CreateOrthographicOffCenter(
                    0, (float)screenWidth, (float)screenHeight, 0, 0, 1);


            basicEffect = new BasicEffect(graphicsDevice, null);

            basicEffect.Projection = projectionMatrix;

            basicEffectVertexDeclaration = new VertexDeclaration(
                graphicsDevice, VertexPositionColor.VertexElements);

            graphicsDevice.VertexDeclaration =
                basicEffectVertexDeclaration;
        }

        #endregion
    }
}
