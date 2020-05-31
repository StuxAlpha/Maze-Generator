using System;
using System.Collections.Generic;
using System.Text;

namespace Maze_Generator
{
    //Represents a line (coloumn or row) of the maze.
    class Line
    {
        //How many spaces along from top/left the line is in the maze
        public int Position
        { get; }

        //Each element represents a possible path intersecting the line.
        //True = wall
        //False = open
        private bool[] paths;
        public bool[] Paths
        {
            get { return paths; }
            private set { paths = value; }
        }

        //Whether the path states for this line have been locked, using the LineLocked() method.
        private bool locked;
        public bool Locked
        {
            get { return locked; }
            private set { locked = value; }
        }

        public Line(int position, int pathNumber)
        {
            this.Position = position;
            this.Paths = new bool[pathNumber];
            for (int i = 0; i < Paths.Length; i++){ Paths[i] = true; }
            locked = false;
        }

        //Used to set the path states for the intersecting paths of this line.
        //This is determined by checking which intersecting lines have already been locked,
        //Then ensuring there is exactly one open path in each partition between each locked intersecting line.
        public void LockLine(Line[] intersectingLines)
        {
            if (locked == false)
            {
                Random rng = new Random();
                List<int> partitionSizes = new List<int>() { 1 };
                List<int> partitionLocations = new List<int>() { 0 };

                for (int i = 0; i < intersectingLines.Length; i++)
                {
                    if(intersectingLines[i].Locked == false)
                    
                        
                        {
                            partitionSizes[partitionSizes.Count - 1]++;
                        }
                        else
                        {  
                            partitionLocations.Add(i+1);
                            partitionSizes.Add(1);
                        }
                }

                for (int i = 0; i < partitionSizes.Count; i++)
                {
                    int gapSubPosition = rng.Next(partitionSizes[i]);
                    int gapAbsolutePosition = partitionLocations[i] + gapSubPosition;
                    Paths[gapAbsolutePosition] = false;
                }
            }

            locked = true;
        }
    }
}
