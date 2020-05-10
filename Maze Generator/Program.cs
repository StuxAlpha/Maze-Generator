using System;
using System.Collections.Generic;

namespace Maze_Generator
{
    class Program
    {
        static void Main(string[] args)
        {
            //This program will generate a sqaure grid maze of a size specified by the user.
            //The maze will take the form of a two dimensional array of type bool, with false representing a path and false representing a wall.
            //The program also prints a visual representation to the console.
            //The maze will be enclosed by walls.
            //The maze will have an entrance and exit in the outer walls, on randomly chosen different edges.
            //The maze will always be solvable - every path location will be contiguous with all other path locations.
            //The algorithm will never generate loops in the maze.
            //The mazes generated have low depth but hight breadth - that is to say that the total distance to the exit will be relatively short, but that there are many branching paths.

            //Below is an empty maze, before the algorithm adds additional walls and creates the entrance and exit:

            //                      c1  c2  c3  c4
            //                    w1  w2  w3  w4  w5
            //                  ██████████████████████
            //               h1 ██                  ██
            //            r1    ██  ██  ██  ██  ██  ██
            //               h2 ██                  ██
            //            r2    ██  ██  ██  ██  ██  ██
            //               h3 ██                  ██
            //            r3    ██  ██  ██  ██  ██  ██
            //               h4 ██                  ██
            //            r4    ██  ██  ██  ██  ██  ██
            //               h5 ██                  ██
            //                  ██████████████████████

            //The maze already contains a number of pillars separating the paths in the maze. The algorithm simply fills in many of the gaps between these pillars with more walls.
            //Due to this, the maze must always have a total height and width that are odd numbers, as every other row or column will have strictly alternating wall then open path.
            //This is necessary for the algorith to function.

            //For this reason, when the user specificies the size of the maze instead of setting the absolute width and height they will set the number of paths.
            //In the example above, this is represented by w1 to w5 and h1 to h5, making it a maze of width 5 and height 5.

            //In this program:
            //- We will refer to the number of paths wide or high as width or height of the maze.
            //- We will refer to the number of lines of pillars as the number of columns or rows of the maze.
            //- We will refer to the total number of cells the final maze will be high or wide as the absolute height or absolute width.

            //SUMMARY OF THE ALGORITHM:
            //Using the above as a starting point, the algorith first chooses a column or row. It fills this line with walls across the whole maze, leaving a single gap on a path.
            //This means that either side of this line is connected, and so the maze paths are contiguous.
            //Next a perpendicular line (if the first was a column, the second will be a row) is randomly selected, and again the line is filled in as walls,
            //But this time a gap is left both before and after the previous line.
            //In this way we now have 4 segments, all of which are guaranteed to be connected.
            //This continues, alternating between rows and columns, with a gap always left between any two bisecting lines each time a line is filled in.
            //If there are more columns than rows or vice versa, the remaining ones are filled without alternating orientation.
            //Because at every stage it is a logical necessity that all spaces remain connected, this must also be true of the final maze.

            //First we allow the user to specify the width and height of the maze.
            Console.WriteLine("How many paths wide should the maze be?");
            int width = int.Parse(Console.ReadLine());

            Console.WriteLine("How many paths high should the maze be?");
            int height = int.Parse(Console.ReadLine());

            //This array will hold the finished maze. As explained above, the absolute size is twice the number of paths + 1, for each of the two dimensions.
            bool[,] maze = new bool[width * 2 + 1, height * 2 + 1];

            //We set the top and bottom rows of the maze to be true, to create the top and bottom walls of the maze.
            for (int i = 0; i < width * 2 + 1; i++)
            {
                maze[i, 0] = true;
                maze[i, height * 2] = true;
            }

            //We do the same for the left and right sides.
            for (int i = 0; i < height * 2 + 1; i++)
            {
                maze[0, i] = true;
                maze[width * 2, i] = true;
            }

            //Then we add in the pillars of maze
            for (int i = 0; i < width - 1; i++)
            {
                for (int j = 0; j < height - 1; j++)
                {
                    maze[i * 2 + 2, j * 2 + 2] = true;
                }
            }

            //Here we create two arrays. One stores all the columns of the maze, the other all of the rows.
            //Columns and Rows are created as instances of the Line Class.
            //An instance of the Line Class is defined by its membershio of either the rows or columns array,
            //and by its properties expressing its position in the maze (expressed in the example as c1... and r1... ) and how many paths cross perpendicular to it.
            //By our definition of the maze, we will have one fewer columns than the width of the maze, and there will be a number of paths across it equal to the height.
            //For rows, there will be one fewer than the height of the maze and there will be a number of paths across it equal to the width.

            //Lines are stored as a custom class rather than a simple array due to their method Lockline(), which will be explained elsewhere.

            Line[] columns = new Line[width - 1];
            for(int i = 0; i < columns.Length; i++)
            {
                columns[i] = new Line(i, height);
            }

            Line[] rows = new Line[height - 1];
            for (int i = 0; i < rows.Length; i++)
            {
                rows[i] = new Line(i, width);
            }

            //These lists will store the indexes of columns and rows, relating to the columns and rows arrays, that have not yet had their Lockline() method invoked.
            //Initially, this will be ALL rows and columns.
            List<int> columnsLeft = new List<int>();
            for (int i = 0; i < columns.Length; i++)
            {
                columnsLeft.Add(i);
            }

            List<int> rowsLeft = new List<int>();
            for (int i = 0; i < rows.Length; i++)
            {
                rowsLeft.Add(i);
            }

            //Here we randomise the order of the lists - this will be the order that the columns and rows are filled in the maze,
            //and one of the core bases for the randomisation of the maze.
            Shuffler.Shuffle(columnsLeft);
            Shuffler.Shuffle(rowsLeft);

            //Create the random object that will be used for further randomised elements.
            Random rng = new Random();
            
            //Used to store the index of the next Line object that we will invoke the Lockline() method.
            int nextLineIndex;

            //Used to randomise whether the first Line will be a column or a row.
            int coinFlip = rng.Next(2);

            //Here we iterate across all columns and rows, alternating until this is no longer possible, using the Lockline() method on each Line.
            //This is the implementation of the algorith as explained in the initial summary.
            for (int i = 0; i < Math.Max(columns.Length,rows.Length); i++)
            {
                //as a coing flip of 0 skips the first pass on columns, if there are more columns than rows there will need to be an extra run through the loop to build it.
                if(coinFlip == 0 && columns.Length >= rows.Length)
                { i--; }
                
                //skips the first pass on columns if coinflip is 0, thus making the first line a row.
                if (coinFlip > 0)
                {
                    if (columnsLeft.Count > 0)
                    {
                        //reads the first element of columnsLeft, looks up that as an index in the set of all columns, then locks that line. Finally, removes the first element from columnsLeft
                        nextLineIndex = columnsLeft[0];
                        columns[nextLineIndex].lockLine(rows);
                        columnsLeft.RemoveAt(0);
                    }
                }

                if (rowsLeft.Count > 0)
                {
                    //same operation as above, but for rows.
                    nextLineIndex = rowsLeft[0];
                    rows[nextLineIndex].lockLine(columns);
                    rowsLeft.RemoveAt(0);
                }
                
                //stops the loop from skipping columns on subsequent passes
                coinFlip = 1;
            }

            //Here we read from each Line in columns. The Paths property of each Line is an array of bool, that represents whether each path that crosses that line is open or a wall.
            //As our final maze uses the same typing, we can simply copy over the value to the respective cell of the maze.
            for (int i = 0; i < columns.Length; i++)
            {
                for (int j = 0; j < columns[i].Paths.Length; j++)
                {
                    maze[i * 2 + 2, j * 2 + 1] = columns[i].Paths[j];
                }
            }

            //The same is done for rows.
            for (int j = 0; j < rows.Length; j++)
            {
                for (int i = 0; i < rows[j].Paths.Length; i++)
                {
                    maze[i * 2 + 1, j * 2 + 2] = rows[j].Paths[i];

                }
            }

            //This section creates and entrance and exit for the maze, by replacing the walls in two cells on different edges with an open gap.
            //To guarantee that this links to an open cell of the maze, it must be in line with the paths rather than the rows and columns, as any given cell on a row or column may become a wall.
            
            //These arrays hold the details for the locations of the entrance and exit. For each, the first int represents which side of the maze it is on, and the second what position along that side.
            int[] mazeEntrance = new int[2];
            int[] mazeExit = new int[2];
  
            //We randomly determine which of the 4 sides the entrance and exit are on.
            mazeEntrance[0] = rng.Next(4);
            mazeExit[0] = rng.Next(4);
            //If the exit and entrance are on the same side, we reassign the exit's side until it is different.
            while (mazeExit[0] == mazeEntrance[0])
            {
                mazeExit[0] = rng.Next(4);
            }

            //For the exit and entrance, we randomly determine a valid position along the side set for it, then remove the wall on that cell of the maze.
            int[][] entranceAndExit = { mazeEntrance, mazeExit };
            foreach(int[] entranceOrExit in entranceAndExit)
            {
                switch (entranceOrExit[0])
                {
                    case 0:
                        entranceOrExit[1] = rng.Next(width);
                        maze[entranceOrExit[1] * 2 + 1, 0] = false;
                        break;
                    case 1:
                        entranceOrExit[1] = rng.Next(height);
                        maze[0, entranceOrExit[1] * 2 + 1] = false;
                        break;
                    case 2:
                        entranceOrExit[1] = rng.Next(width);
                        maze[entranceOrExit[1] * 2 + 1, height*2] = false;
                        break;
                    case 3:
                        entranceOrExit[1] = rng.Next(height);
                        maze[width*2, entranceOrExit[1] * 2 + 1] = false;
                        break;
                    default:
                        Console.WriteLine("entrance/exit error");
                        break;

                }
            }

            //Finally we print maze to console!
            //A double character is used for each cell of the maze, to make each cell approximately square for the visual output.
            string rowToPrint = "";
            for (int i = 0; i < maze.GetLength(1); i++)
            {
                for (int j = 0; j < maze.GetLength(0); j++)
                {
                    if (maze[j,i] == true)
                    {
                        rowToPrint = rowToPrint + "██";
                    }
                    else
                    {
                        rowToPrint = rowToPrint + "  ";
                    }
                }
                Console.WriteLine(rowToPrint);
                rowToPrint = "";
            }
        }
    }


}
