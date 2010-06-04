using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Bounce
{
    public class Ball
    {
        #region Fields

        private Vector3 position;
        private Vector3 velocity;
        private int radius;

        private Texture2D texture;

        #endregion

        #region Properties

        public Vector3 Position
        {
            get { return position; }
            set { position = value; }
        }
        public Vector3 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }

        public int Radius
        {
            get { return radius; }
        }

        public Rectangle Bbox
        {
            get { return 
                new Rectangle((int) position.X - radius, (int) position.Y - radius,
                    radius * 2, radius * 2); }
        }

        #endregion

        #region Initialization

        public Ball(Vector3 position, Vector3 velocity, int radius)
        {
            this.position = position;
            this.velocity = velocity;
            this.radius = radius;
        }

        public void LoadContent(ContentManager content)
        {
            //texture = content.Load<Texture2D>("Player/ball");
        }

        #endregion

        #region Update and Draw

        public void Update(GameTime gameTime)
        {
            float elapsedTime = gameTime.ElapsedGameTime.Milliseconds / 1000f;
            position += velocity * elapsedTime;
        }

        public void Draw(Camera camera)
        {
            VertexPositionColor[] pointList = new VertexPositionColor[21];

            for (int i = 0; i < 360; i+=18)
            {
                Vector3 point = new Vector3((float)(radius * Math.Cos((2.0 * Math.PI / 360) * i)),
                                    (float)(radius * Math.Sin((2.0 * Math.PI / 360) * i)), 0f);
                point += position;
                pointList[i/18] = new VertexPositionColor(point, Color.Black);
            }
            pointList[20] = new VertexPositionColor(position, Color.Black);

            short[] triangleListIndices = { 20, 0, 1, 20, 1, 2, 20, 2, 3, 20, 3, 4, 20, 4, 5, 
                                              20, 5, 6, 20, 6, 7, 20, 7, 8, 20, 8, 9, 
                                              20, 9, 10, 20, 10, 11, 20, 11, 12, 20, 12, 13,
                                              20, 13, 14, 20, 14, 15, 20, 15, 16, 20, 16, 17,
                                              20, 17, 18, 20, 18, 19, 20, 19, 0};
            camera.BaseEffect.Begin();

            foreach (EffectPass pass in camera.BaseEffect.CurrentTechnique.Passes)
            {
                pass.Begin();
                    camera.GrapicsDevice.DrawUserIndexedPrimitives<VertexPositionColor>(
                    PrimitiveType.TriangleList,
                    pointList,
                    0,   // vertex buffer offset to add to each element of the index buffer
                    21,   // number of vertices to draw
                    triangleListIndices,
                    0,   // first index element to read
                    20    // number of primitives to draw
                    );
                 
                pass.End();
            }

            camera.BaseEffect.End();
            
                
                /*spriteBatch.Draw(texture, position, null, Color.White, 0.0f, 
                new Vector2(texture.Width / 2, texture.Height / 2), 1.0f,
                SpriteEffects.None, 0.0f); */
        }

        #endregion

        #region Public Methods

        public void move(Vector3 direction)
        {
            position += direction;
        }

        #endregion


    }
}
