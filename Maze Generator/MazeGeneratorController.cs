using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using System.Dynamic;

namespace Maze_Generator
{
    static class MazeGeneratorController
    {
        public static void PrintMazeToConsole(bool[,] maze)
        {
            string rowToPrint = "";
            for (int i = 0; i < maze.GetLength(1); i++)
            {
                for (int j = 0; j < maze.GetLength(0); j++)
                {
                    if (maze[j, i] == true)
                    {
                        rowToPrint += "██";
                    }
                    else
                    {
                        rowToPrint += "  ";
                    }
                }
                Console.WriteLine(rowToPrint);
                rowToPrint = "";
            }
        }

        public static void SaveMazeAsJPG(bool[,] maze)
        {
            int width = maze.GetLength(0);
            int height = maze.GetLength(1);

            
            var output = new Bitmap(width, height);

            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                {

                    if (maze[x, y] == true)
                    {

                        output.SetPixel(x, y, Color.Black);
                    }
                    else
                    {
                        output.SetPixel(x, y, Color.White);
                    }
                }

            output.Save(width + "x" + height + "Maze.jpg", ImageFormat.Jpeg);
        }


        public static bool[,] GenerateMaze(int width, int height)
        {
            bool[,] maze = new bool[width * 2 + 1, height * 2 + 1];
            Random rng = new Random();
            int nextLineIndex;

            //Used to randomise whether the first Line will be a column or a row.
            int coinFlip = rng.Next(2);

            //Initialises the maze with walls and pillars
            for (int i = 0; i < width * 2 + 1; i++)
            {
                maze[i, 0] = true;
                maze[i, height * 2] = true;
            }

            for (int i = 0; i < height * 2 + 1; i++)
            {
                maze[0, i] = true;
                maze[width * 2, i] = true;
            }

            for (int i = 0; i < width - 1; i++)
            {
                for (int j = 0; j < height - 1; j++)
                {
                    maze[i * 2 + 2, j * 2 + 2] = true;
                }
            }

            //Create the rows and columns of the maze
            Line[] columns = new Line[width - 1];

            for (int i = 0; i < columns.Length; i++)
            {
                columns[i] = new Line(i, height);
            }

            Line[] rows = new Line[height - 1];

            for (int i = 0; i < rows.Length; i++)
            {
                rows[i] = new Line(i, width);
            }

            //Create shuffled lists of the indexes of the rows and columns, to allow us to randomise the order they are selected in the algorithm
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
            Shuffler.Shuffle(columnsLeft);
            Shuffler.Shuffle(rowsLeft);

            //Invoke the Lockline method of every line, alternating rows and columns, in the shuffled order.
            for (int i = 0; i < Math.Max(columns.Length, rows.Length); i++)
            {
                if (coinFlip == 0 && columns.Length >= rows.Length)
                { i--; }

                if (coinFlip == 1)
                {
                    if (columnsLeft.Count > 0)
                    {
                        nextLineIndex = columnsLeft[0];
                        columns[nextLineIndex].LockLine(rows);
                        columnsLeft.RemoveAt(0);
                    }
                }

                if (rowsLeft.Count > 0)
                {
                    nextLineIndex = rowsLeft[0];
                    rows[nextLineIndex].LockLine(columns);
                    rowsLeft.RemoveAt(0);
                }
                coinFlip = 1;
            }

            //Read the blocked paths from the columns and rows into the maze
            for (int i = 0; i < columns.Length; i++)
            {
                for (int j = 0; j < columns[i].Paths.Length; j++)
                {
                    maze[i * 2 + 2, j * 2 + 1] = columns[i].Paths[j];
                }
            }

            for (int j = 0; j < rows.Length; j++)
            {
                for (int i = 0; i < rows[j].Paths.Length; i++)
                {
                    maze[i * 2 + 1, j * 2 + 2] = rows[j].Paths[i];

                }
            }

            //Creates and entrance and exit for the maze, by replacing the walls in two cells on different edges with an open gap.
            int[] mazeEntrance = new int[2];
            int[] mazeExit = new int[2];

            mazeEntrance[0] = rng.Next(4);
            mazeExit[0] = rng.Next(4);

            while (mazeExit[0] == mazeEntrance[0])
            {
                mazeExit[0] = rng.Next(4);
            }

            int[][] entranceAndExit = { mazeEntrance, mazeExit };
            foreach (int[] entranceOrExit in entranceAndExit)
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
                        maze[entranceOrExit[1] * 2 + 1, height * 2] = false;
                        break;
                    case 3:
                        entranceOrExit[1] = rng.Next(height);
                        maze[width * 2, entranceOrExit[1] * 2 + 1] = false;
                        break;
                    default:
                        Console.WriteLine("entrance/exit error");
                        break;

                }


            }

            return maze;


        }
    }
}
