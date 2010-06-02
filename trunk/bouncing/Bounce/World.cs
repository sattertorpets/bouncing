using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Bounce
{
    public class World
    {
        #region Fields

        private int screenHeight;
        private int screenWidth;
        private CollissionEngine worldCollisions;
        private Ball ball = new Ball(new Vector3(200, 250, 0), new Vector3(-20, 20, 0), 20);
        private Camera camera;

        #endregion

        #region Initialization

        public World(int screenWidth, int screenHeight, GraphicsDevice graphicsDevice)
        {
            this.screenHeight = screenHeight;
            this.screenWidth = screenWidth;
            worldCollisions = new Bounce.CollissionEngine(200, screenWidth, screenHeight);
            camera = new Camera(screenHeight, screenWidth, graphicsDevice);
        }

        public void LoadContent(ContentManager content)
        {
            ball.LoadContent(content);
        }

        #endregion

        #region Update and Draw

        public void Update(GameTime gameTime)
        {
            ball.Update(gameTime);
            worldCollisions.collission(ball);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            ball.Draw(camera);
            worldCollisions.Draw(camera);

            spriteBatch.End();
        }

        #endregion
    }
}
