using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Bounce
{
    public class Segment
    {
        #region Fields

        private Vector2 startPoint;
        private Vector2 endPoint;

        #endregion

        #region Properties

        public Vector2 StartPoint
        {
            get { return startPoint; }
        }

        public Vector2 EndPoint
        {
            get { return endPoint; }
        }

        #endregion

        #region Initialization

        public Segment(Vector2 start, Vector2 end)
        {
            startPoint = start;
            endPoint = end;
        }

        #endregion

        #region Public Methods

        public float angleToYAxis()
        {
            float x = startPoint.X - endPoint.X;
            float y = startPoint.Y - endPoint.Y;

            return (float) Math.Atan2(x, y);
        }

        #endregion

    }
}
