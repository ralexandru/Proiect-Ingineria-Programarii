using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proiect_IP_w_UI
{
    internal class Utilities
    {
        public enum Direction
        {
            None,
            Up,
            Down,
            Left,
            Right
        }

        public static int[,] GetMazeFromFile(string filePath)
        {
            // Se citeste matricea binara din fisier
            string[] stringMatrix = File.ReadAllLines(filePath);

            // Se determina numarul de linii si coloane
            int noOfLines = stringMatrix.Length;
            int noOfColumns = stringMatrix.Max(line => line.Split(' ').Length);

            // Se initializeaza matricea de intregi
            int[,] matrix = new int[noOfLines, noOfColumns];

            // Se copieaza matricea din fisier in matricea de intregi
            for (int i = 0; i < noOfLines; i++)
            {
                string[] values = stringMatrix[i].Split(' ');
                for (int j = 0; j < values.Length; j++)
                {
                    matrix[i, j] = Convert.ToInt32(values[j]);
                }
            }
            return matrix;
        }

        // Punct de plecare al metodei de rezolvare
        public List<List<Position>> FindPath()
        {
            // Lista ce contine toate solutiile
            List<List<Position>> allSolutions = new List<List<Position>>();
            // Lista ce contine toate intrarile in labirint
            List<Position> entrances = FindEntrances(GetMazeFromFile("maze.txt"));

            // Drumul explorat
            List<Position> currentPath = new List<Position>();
            currentPath.Add(entrances[0]);

            // Se initializeaza matricea de celule vizitate
            int[,] visitedNodes = new int[GetMazeFromFile("maze.txt").GetLength(0), GetMazeFromFile("maze.txt").GetLength(1)];
            // Se apeleaza functia recursiva
            Direction lastMove = Direction.None;
            if (ExploreMaze(GetMazeFromFile("maze.txt"), entrances[0], entrances[0], 0, lastMove, currentPath, visitedNodes))
                //Daca solutia este gasita, se adauga la lista de solutii
                allSolutions.Add(new List<Position>(currentPath));

            // Se printeaza solutiile pentru debugging
            Debug.WriteLine("Solutions");
            foreach (List<Position> solution in allSolutions)
                foreach (Position position in solution)
                    Debug.WriteLine(position);

            return allSolutions;
        }

        List<Position> FindEntrances(int[,] maze)
        {
            List<Position> entrances = new List<Position>();
            int rows = maze.GetLength(0);
            int columns = maze.GetLength(1);
            // Verifica laturile de sus si de jos ale matricei
            for (int j = 0; j < columns; j++)
            {
                if (maze[0, j] == 0)
                {
                    Position entrance = new Position(0, j);
                    entrances.Add(entrance);
                }
                if (maze[rows - 1, j] == 0)
                {
                    Position entrance = new Position(rows - 1, j);
                    entrances.Add(entrance);
                }
            }

            // Verifica laturile din stanga si dreapta ale matricei
            for (int i = 1; i < rows - 1; i++)
            {
                if (maze[i, 0] == 0)
                {
                    Position entrance = new Position(i, 0);
                    entrances.Add(entrance);
                }
                if (maze[i, columns - 1] == 0)
                {
                    Position entrance = new Position(i, columns - 1);
                    entrances.Add(entrance);
                }
            }
            return entrances;
        }


        bool ExploreMaze(int[,] matrix, Position currentPosition, Position entrance, int moves, Direction lastMove, List<Position> path, int[,] visitedNodes)
        {
            Debug.WriteLine("Current position: " + currentPosition.ToString() + " | Value of element: " + matrix[currentPosition.x, currentPosition.y]);
            bool foundExit = false;

            // Conditii de iesire - S-a gasit iesirea
            if (currentPosition.x == 0 && moves > 0)
                foundExit = true;
            if (currentPosition.x == matrix.GetLength(0) - 1 && moves > 0)
                foundExit = true;
            if (currentPosition.y == 0 && moves > 0)
                foundExit = true;
            if (currentPosition.y == matrix.GetLength(1) - 1 && moves > 0)
                foundExit = true;

            // Incheierea functiei recursive
            if (foundExit && (currentPosition.x != entrance.x || currentPosition.y != entrance.y))
            {
                Debug.WriteLine("Found exit!");
                return true;
            }
                moves++;
                // Se verifica daca celula din stanga poate fi vizitata
                if (currentPosition.y - 1 >= 0 && matrix[currentPosition.x, currentPosition.y - 1] == 0 && visitedNodes[currentPosition.x, currentPosition.y - 1] == 0 && lastMove != Direction.Right)
                {
                    Position nextPosition = new Position(currentPosition.x, currentPosition.y - 1);
                    path.Add(nextPosition); // Se adauga celula vizitata la drumul explorat
                    visitedNodes[currentPosition.x, currentPosition.y - 1] = 1;
                    lastMove = Direction.Left; // Se memoreaza ultima miscare efectuata                
                    if (ExploreMaze(matrix, nextPosition, entrance, moves, lastMove, path, visitedNodes))
                        return true; // Iesirea a fost gasita in apelul recursiv
                    path.Remove(nextPosition); // Backtrack
                    lastMove = Direction.None;
                }

                // Se verifica daca celula din dreapta poate fi vizitata
                if (currentPosition.y + 1 < matrix.GetLength(1) && matrix[currentPosition.x, currentPosition.y + 1] == 0 && visitedNodes[currentPosition.x, currentPosition.y + 1] == 0 && lastMove != Direction.Left)
                {
                    Position nextPosition = new Position(currentPosition.x, currentPosition.y + 1);
                    path.Add(nextPosition); // Se adauga celula vizitata la drumul explorat
                    visitedNodes[currentPosition.x, currentPosition.y + 1] = 1;
                    lastMove = Direction.Right; // Se memoreaza ultima miscare efectuata   
                    if (ExploreMaze(matrix, nextPosition, entrance, moves, lastMove, path, visitedNodes))
                        return true; // Iesirea a fost gasita in apelul recursiv
                    path.Remove(nextPosition); // Backtrack
                    lastMove = Direction.None;
                }

                // Se verifica daca celula de jos poate fi vizitata
                if (currentPosition.x + 1 < matrix.GetLength(0) && matrix[currentPosition.x + 1, currentPosition.y] == 0 && visitedNodes[currentPosition.x + 1, currentPosition.y] == 0 && lastMove != Direction.Up)
                {
                    Position nextPosition = new Position(currentPosition.x + 1, currentPosition.y);
                    path.Add(nextPosition); // Se adauga celula vizitata la drumul explorat
                    visitedNodes[currentPosition.x + 1, currentPosition.y] = 1;
                    lastMove = Direction.Down; // Se memoreaza ultima miscare efectuata   
                    if (ExploreMaze(matrix, nextPosition, entrance, moves, lastMove, path, visitedNodes))
                        return true; // Iesirea a fost gasita in apelul recursiv
                    path.Remove(nextPosition); // Backtrack
                    lastMove = Direction.None;
                }

                // Se verifica daca celula de sus poate fi vizitata
                if (currentPosition.x - 1 >= 0 && matrix[currentPosition.x - 1, currentPosition.y] == 0 && visitedNodes[currentPosition.x - 1, currentPosition.y] == 0 && lastMove != Direction.Down)
                {
                    Position nextPosition = new Position(currentPosition.x - 1, currentPosition.y);
                    path.Add(nextPosition); // Se adauga celula vizitata la drumul explorat
                    visitedNodes[currentPosition.x - 1, currentPosition.y] = 1;
                    lastMove = Direction.Up; // Se memoreaza ultima miscare efectuata   
                    if (ExploreMaze(matrix, nextPosition, entrance, moves, lastMove, path, visitedNodes))
                        return true; // Iesirea a fost gasita in apelul recursiv
                    path.Remove(nextPosition); // Backtrack
                    lastMove = Direction.None;
                }
                return false;
            }
        }
    }
