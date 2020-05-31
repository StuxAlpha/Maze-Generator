using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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

            MazeGeneratorController.PrintMazeToConsole(maze);

            string input = "";

            while (input != "y" && input != "n")
            {
                
                Console.WriteLine("Would you like to save this maze as an image? y or n.");
                input = Console.ReadLine();
                if (input == "Y" || input == "y")
                {
                    MazeGeneratorController.SaveMazeAsJPG(maze);
                }
                else
                {
                    if (input != "N" && input != "n")
                    {
                        Console.WriteLine("Please type y or n");
                    }
                }
                input.ToLower();
            }

        }
    }


}
