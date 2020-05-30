# Maze-Generator

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
