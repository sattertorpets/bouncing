using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace Bounce
{
    public class CollissionEngine
    {
        #region Fields

        private List<Segment> allSegments = new List<Segment>();
        private List<Segment>[,] segmentGrid;
        private int worldHeight;
        private int worldWidth;
        private int gridsize;


        #endregion

        #region Initialization

        public CollissionEngine(int gridsize, int worldHeight, int worldWidth)
        {
            this.worldHeight = worldHeight;
            this.worldWidth = worldWidth;
            this.gridsize = gridsize;

            //load segments from file
            LoadSegments();

            segmentGrid = new List<Segment>[worldWidth / gridsize, worldHeight / gridsize];

            for (int i = 0; i < worldWidth / gridsize; i++)
            {
                for (int j = 0; j < worldHeight / gridsize; j++)
                {
                    segmentGrid[i, j] = new List<Segment>();
                }
            }

            //Put segments in all grid squares they cover.
            foreach (Segment segment in allSegments)
            {
                int x1 = (int) segment.StartPoint.X / gridsize;
                int y1 = (int) segment.StartPoint.Y / gridsize;

                int x2 = (int) segment.EndPoint.X / gridsize;
                int y2 = (int) segment.EndPoint.Y / gridsize;

                for(int i = x1; i <= x2; i++)
                    for (int j = y1; j <= y2; j++)
                    {
                        segmentGrid[i, j].Add(segment);
                    }
            }
        }

        #endregion

        #region Draw

        public void Draw(Camera camera)
        {
            foreach (Segment segment in allSegments)
            {
                VertexPositionColor[] pointList = new VertexPositionColor[2];

                pointList[0] = new VertexPositionColor(
                            new Vector3(segment.StartPoint, 0), Color.Black);

                pointList[1] = new VertexPositionColor(
                            new Vector3(segment.EndPoint, 0), Color.Black);

                // Initialize an array of indices of type short.
                short[] lineListIndices = new short[2];

                lineListIndices[0] = 0;
                lineListIndices[1] = 1;

                camera.BaseEffect.Begin();

                foreach (EffectPass pass in camera.BaseEffect.CurrentTechnique.Passes)
                {
                    pass.Begin();

                    camera.GrapicsDevice.DrawUserIndexedPrimitives<VertexPositionColor>(
                        PrimitiveType.LineList,
                        pointList,
                        0,  // vertex buffer offset to add to each element of the index buffer
                        2,  // number of vertices in pointList
                        lineListIndices,  // the index buffer
                        0,  // first index element to read
                        1   // number of primitives to draw
                        );

                    pass.End();
                }

                camera.BaseEffect.End();
            }
        }

        #endregion

        #region Public Methods

        public void collission(Ball ball)
        {
            int x1 = ball.Bbox.Left / gridsize;
            int y1 = ball.Bbox.Top / gridsize;

            int x2 = ball.Bbox.Right / gridsize;
            int y2 = ball.Bbox.Bottom / gridsize;

            List<Segment> segments = new List<Segment>();

            //Collect all segments the boundingbox of the ball has in it.
            for (int i = x1; i <= x2; i++)
                for (int j = y1; j <= y2; j++)
                {
                    segments.AddRange(segmentGrid[i, j]);
                }

            //Go through all segments
            foreach (Segment segment in segments)
            {
                //Find angle between segment and Y-axis
                float angle = segment.angleToYAxis();
                //Rotate balls middle point according to segments angle to Y-axis
                Vector3 rotatedPoint = ball.Position;
                Matrix transform =
                    Matrix.CreateTranslation(new Vector3(-segment.StartPoint, 0.0f)) *
                    Matrix.CreateRotationZ(angle) *
                    Matrix.CreateTranslation(new Vector3(segment.StartPoint, 0.0f));
                rotatedPoint = Vector3.Transform(rotatedPoint, transform);

                //If ball is to the right of segment
                if (Colliding(rotatedPoint, (int)segment.StartPoint.X, ball.Radius))
                {
                    Vector3 velocity = ball.Velocity;

                    int moves = 0;
                    Vector3 direction = -velocity / 10;
                    while (Colliding(rotatedPoint, (int)segment.StartPoint.X, ball.Radius))
                    {
                        ball.move(direction);
                        moves ++;
                        rotatedPoint = ball.Position;
                        rotatedPoint = Vector3.Transform(rotatedPoint, transform);
                    }

                    velocity = Vector3.Transform(velocity, Matrix.CreateRotationZ(angle));
                    velocity.X = -velocity.X;
                    velocity = Vector3.Transform(velocity, Matrix.CreateRotationZ(-angle));
                    ball.Velocity = velocity;

                    for (int i = 0; i <= moves; i++)
                    {
                        ball.move(velocity / 10);
                    }
                }
            }
        }

        #endregion

        #region Private Methods

        private bool Colliding(Vector3 position, int X, int radius)
        {
            if (position.X <= X && position.X + radius >= X)
                return true;
            else if (position.X > X && position.X - radius <= X)
                return true;
            else
                return false;
        }

        private void LoadSegments()
        {
            //Try to read file segments.txt
            try
            {
                TextReader tr = new StreamReader("segments.txt");
                char[] separatorX = new char[] { 'x' };
                char[] separatorSpace = new char[] { ' ' };

                string line = tr.ReadLine();
                while (line != null)
                {
                    //Split line at space, there should be two values for each line
                    string[] values = line.Split(separatorSpace);
                    if (values.Length == 2)
                    {
                        Vector2 startPoint = Vector2.Zero;
                        Vector2 endPoint = Vector2.Zero;

                        //Split first value at character x, there should be two new values
                        string[] vecValues = values[0].Split(separatorX);
                        if (vecValues.Length == 2)
                        {
                            //Create a vector from the two values
                            try
                            {
                                float val1 = float.Parse(vecValues[0]);
                                float val2 = float.Parse(vecValues[1]);
                                startPoint = new Vector2(val1, val2);
                            }
                            catch (FormatException)
                            {
                                Console.WriteLine("Wrong format on vector.");
                            }
                        }
                        else
                            Console.WriteLine("something wrong with formatting of vector numbers in file");

                        //split the second value at x, there should be two new values
                        vecValues = values[1].Split(separatorX);
                        if (vecValues.Length == 2)
                        {
                            //Create a vector from the two values
                            try
                            {
                                float val1 = float.Parse(vecValues[0]);
                                float val2 = float.Parse(vecValues[1]);
                                endPoint = new Vector2(val1, val2);
                            }
                            catch (FormatException)
                            {
                                Console.WriteLine("Wrong format on vector.");
                            }
                        }
                        else
                            Console.WriteLine("something wrong with formatting of vector numbers in file");

                        //Add the two vectors as a segment.
                        allSegments.Add(new Segment(startPoint, endPoint));

                    }
                    else
                        Console.WriteLine("Too many values on a line");

                    //Read next line
                    line = tr.ReadLine();
                }
                //Close line reader
                tr.Close();

            }
            catch (FileNotFoundException)
            {

            }
        }


        #endregion
    }

    
}
