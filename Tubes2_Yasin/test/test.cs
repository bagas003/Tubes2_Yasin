using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;


namespace Tubes2_Yasin
{
    public class Program
    {
        public Tuple<int, int> start = null, current = null;
        public int TCounts = 0, TFounds = 0, timespan = 100;
        public char[,] map = null;
        public bool[,] visited = null;
        public Tuple<int, int> [,] parent = null;
        public List<Tuple<int, int>> path = new List<Tuple<int, int>>();

        public int[] dx = { 1, 0, -1, 0 };
        public int[] dy = { 0, 1, 0, -1 };
        // PRIORITAS GERAKAN D - R - U - L

        public void fileInput(string filePath) 
        {
            // Read the contents of the file
            string fileContents = File.ReadAllText(filePath);

            // Split the file contents into lines
            string[] lines = fileContents.Split('\n');

            // Determine the number of rows and columns
            int numRows, numCols;
            numRows = numCols = lines.Length;

            // Initialize map
            map = new char[numRows, numCols];

            // Populate the map with the file contents
            for (int i = 0; i < numRows; i++)
            {
                for (int j = 0; j < numCols; j++)
                {
                    map[i, j] = lines[i][j];
                    if (map[i,j] == 'K') 
                    {
                        start = Tuple.Create(i,j);
                    }
                    if (map[i,j] == 'T') 
                    {
                        TCounts++;
                    }
                
                }
            }
        }

        public void printmap() 
        {
            int n = map.GetLength(0);

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (map[i, j] == 'X')
                    {
                        Console.Write(' ');
                    }
                    else if (map[i, j] == 'R')
                    {
                        Console.Write(".");
                    }
                    else
                    {
                        Console.Write(map[i, j]);
                    }
                }
                Console.WriteLine();
            }
        }
    }
}
