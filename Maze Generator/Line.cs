using System;
using System.Collections.Generic;
using System.Text;

namespace Maze_Generator
{
    class Line
    {
        public int Position
        { get; }

        private bool[] paths;
        public bool[] Paths
        {
            get { return paths; }
            private set { paths = value; }
        }

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
        public void lockLine(Line[] intersectingLines)
        {
            if (locked == false)
            {
                Random rng = new Random();
                
                int[] dividingLines = new int[intersectingLines.Length];
                for (int i = 0; i < dividingLines.Length; i++)
                {
                    if (intersectingLines[i].Locked == true)
                    {
                        dividingLines[i] = 1;
                    }
                    else
                    {
                        dividingLines[i] = 0;
                    }
                }
                //used to store the sizes of the consecutive spaces between dividing lines
                List<int> partitionSizes = new List<int>();
                //used to store the starting point of the respective partitions stored in the above
                List<int> partitionLocations = new List<int>();

                partitionLocations.Add(0);
                partitionSizes.Add(1);

                for (int i = 0; i < dividingLines.Length; i++)
                {
                    if(dividingLines[i] == 0)
                    
                        
                        {
                            partitionSizes[partitionSizes.Count - 1]++;
                        }
                        else
                        {
                            
                            partitionLocations.Add(i+1);
                            partitionSizes.Add(1);
                        }
                }

                string reportPartitionLocations = "Partition Locations: ";
                string reportPartitionSizes = "Partition Sizes: ";
                for (int i = 0; i < partitionSizes.Count; i++)
                {
                    reportPartitionSizes = reportPartitionSizes + partitionSizes[i] + " ";
                }
                for (int i = 0; i < partitionLocations.Count; i++)
                {
                    reportPartitionLocations = reportPartitionLocations + partitionLocations[i] + " ";
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
