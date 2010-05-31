using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.IO;

namespace Bounce.World
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

        #region Public Methods

        public void collission(Player.Ball ball)
        {
            int x1 = ball.Bbox.Left;
            int y1 = ball.Bbox.Top;

            int x2 = ball.Bbox.Right;
            int y2 = ball.Bbox.Bottom;

            List<Segment> segments = new List<Segment>();

            for (int i = x1; i <= x2; i++)
                for (int j = y1; j <= y2; j++)
                {
                    segments.AddRange(segmentGrid[i, j]);
                }
            foreach (Segment segment in segments)
            {
                float angle = segment.angleToYAxis();
                Vector2 rotatedPoint = ball.Position;
                Matrix transform =
                    Matrix.CreateTranslation(new Vector3(-segment.StartPoint, 0.0f)) *
                    Matrix.CreateRotationZ(angle) *
                    Matrix.CreateTranslation(new Vector3(segment.StartPoint, 0.0f));
                Vector2.Transform(rotatedPoint, transform);

                if (rotatedPoint.X + ball.Radius <= segment.StartPoint.X)
                {
                    Vector2 velocity = ball.Velocity;
                    Vector2.Transform(velocity, Matrix.CreateRotationZ(angle));
                    velocity.X = -velocity.X;
                    Vector2.Transform(velocity, Matrix.CreateRotationZ(-angle));
                    ball.Velocity = velocity;
                }
            }
        }

        #endregion

        #region Private Methods

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
