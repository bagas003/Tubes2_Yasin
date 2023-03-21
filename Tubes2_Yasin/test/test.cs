using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static Tuple<int, int> start = null, current = null;
    static int TCounts = 0, TFounds = 0;
    static char[,] map = null;
    static bool[,] visited = null;
    static Tuple<int, int> [,] parent = null;
    static List<Tuple<int, int>> path = new List<Tuple<int, int>>();
    
    static int[] dx = { 1, 0, -1, 0 };
    static int[] dy = { 0, 1, 0, -1 };
    // PRIORITAS GERAKAN D - R - U - L
    

    public static void fileInput(string filePath) 
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

    public static void printmap() 
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
    
    public static bool goCheck(int x, int y) 
    {
        return x >= 0 && x < map.GetLength(0) && y >=0 && y < map.GetLength(1) && map[x,y] != 'X' && !visited[x,y];
    }

    public static bool DFS(int x, int y) 
    {
        // mark as visited
        visited[x, y] = true;
        Tuple<int, int> temp = Tuple.Create(x, y);

        // add (x,y) to path
        if (path.Count == 0 || !path.Last().Equals(temp))
        {
            path.Add(temp);
        }

        // collect T if (x,y) is a treasure
        if (map[x, y] == 'T')
        {
            // change T value to R, inc TFounds, change current position
            map[x, y] = 'R';
            TFounds++;
            current = Tuple.Create(x,y);

            // return true
            return true;
        }
        

        // recursive
        for (int i = 0; i < 4; i++) {
            int x2 = x + dx[i];
            int y2 = y + dy[i];
            if (goCheck(x2, y2)) {
                if (DFS(x2, y2)) 
                {
                    return true;
                }
            }
        }

        // remove point from path
        path.RemoveAt(path.Count - 1);
        return false;
    }
    
    public static void BFS(int x, int y)
    {
        // make BFS queue then enque (x,y)
        Queue<Tuple<int, int>> queue = new Queue<Tuple<int, int>>();
        queue.Enqueue(Tuple.Create(x, y));

        // make a matrix of tuple as parent of each points
        parent = new Tuple<int, int>[map.GetLength(0), map.GetLength(1)];
        parent[x, y] = Tuple.Create(-1, -1);

        // loop while queue not empty
        while (queue.Count > 0) {
            // dequeue as temp then mark as visited
            Tuple<int, int> temp = queue.Dequeue();
            visited[temp.Item1, temp.Item2] = true;
            
            // Treasure encountered
            if (map[temp.Item1, temp.Item2] == 'T') {
                // inc Counter, change current position, change map value to R
                TFounds++;
                current = Tuple.Create(temp.Item1, temp.Item2);
                map[temp.Item1, temp.Item2] = 'R';

                // List path from (x,y) to T then concat to path
                List<Tuple<int, int>> tempPath = new List<Tuple<int, int>>();
                while (!temp.Equals(new Tuple<int, int>(x, y))) {
                    tempPath.Add(temp);
                    temp = parent[temp.Item1, temp.Item2];
                }
                tempPath.Reverse();
                path.AddRange(tempPath);
                
                break;
            }

            // enqueue all possible road
            for (int i = 0; i < 4; i++) {
                int x2 = temp.Item1 + dx[i];
                int y2 = temp.Item2 + dy[i];

                if (goCheck(x2, y2))
                {
                    parent[x2, y2] = temp;
                    queue.Enqueue(Tuple.Create(x2, y2));
                }
            }
        }
    }

    public static void solveDFS()
    {
        current = start;
        while (TFounds != TCounts)
        {
            visited = new bool[map.GetLength(0), map.GetLength(1)];
            DFS(current.Item1, current.Item2);
        }
    }

    public static void solveBFS()
    {
        current = start;
        path.Add(start);
        while (TFounds != TCounts)
        {
            visited = new bool[map.GetLength(0), map.GetLength(1)];
            BFS(current.Item1, current.Item2);
        }
    }

    public static string getRoute()
    {
        string route = "";
        Tuple<int, int> temp = null;
        foreach (var item in path)
        {
            if (temp != null)
            {
                if (route != "")
                {
                    route += "- ";
                }
                if (item.Item1 - temp.Item1 == 1)
                {
                    route += "D ";
                }
                if (item.Item1 - temp.Item1 == -1)
                {
                    route += "U ";
                }
                if (item.Item2 - temp.Item2 == 1)
                {
                    route += "R ";
                }
                if (item.Item2 - temp.Item2 == -1)
                {
                    route += "L ";
                }
            }
            temp = item;
        }
        return route;
    }

    // static void Main()
    // {
    //     fileInput("test.txt");
    //     printmap();
        
    //     // solveDFS();
    //     solveBFS();

    //     foreach (var item in path)
    //     {
    //         Console.Write("{0},{1} ", item.Item1, item.Item2);
    //         if (map[item.Item1, item.Item2] == 'R' || map[item.Item1, item.Item2] == 'K')
    //         {
    //             map[item.Item1, item.Item2] = '1';
    //         }
    //         else
    //         {
    //             int temp = Convert.ToInt16(map[item.Item1, item.Item2]);
    //             map[item.Item1, item.Item2] = Convert.ToChar(temp+1);
    //         }
    //     }

    //     Console.WriteLine();
    //     Console.WriteLine(getRoute());

    //     printmap();
    // }
}
