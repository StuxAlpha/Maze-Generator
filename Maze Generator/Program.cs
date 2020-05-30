using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace Maze_Generator
{
    class Program
    {
        static void Main()
        {
            int width;
            int height;

            //Set the width and height of the maze.
            //The number given is the number of wall separated paths long the maze will be in that direction, as opposed to an absolute size.
            Console.WriteLine("How many paths wide should the maze be?");
            string w = Console.ReadLine();
            while (!int.TryParse(w, out width))
            {
                Console.WriteLine("Input must be a number");
                Console.WriteLine("How many paths wide should the maze be?");
                w = Console.ReadLine();
            }

            Console.WriteLine("How many paths high should the maze be?");
            string h = Console.ReadLine();

            while (!int.TryParse(h, out height))
            {
                Console.WriteLine("Input must be a number");
                Console.WriteLine("How many paths high should the maze be?");
                h = Console.ReadLine();
            }

            bool[,] maze = MazeGeneratorController.GenerateMaze(width, height);

            MazeGeneratorController.PrintMaze(maze);

        }
    }


}
