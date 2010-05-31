using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Bounce.Player
{
    public class Ball
    {
        #region Fields

        private Vector2 position;
        private Vector2 velocity;
        private int radius;

        #endregion

        #region Properties

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }
        public Vector2 Velocity
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

        public Ball(Vector2 position, Vector2 velocity, int radius)
        {
            this.position = position;
            this.velocity = velocity;
            this.radius = radius;
        }

        #endregion


    }
}
