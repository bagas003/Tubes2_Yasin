using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using static System.Net.Mime.MediaTypeNames;
using System.Security.RightsManagement;
using System.Windows.Controls.Primitives;
using System.Threading;
using System.Net.NetworkInformation;
using System.Windows.Threading;
using System.Diagnostics;

namespace Tubes2_Yasin
{
    public partial class MainWindow : Window
    {
        public string route;
        public int nodes;
        public int steps;
        public int time;
        private DispatcherTimer _timer;
        private int _currentIndex;
        public State state = new State();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Create an instance of the OpenFileDialog class
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();

            // Set the file filter and other properties
            openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog.Multiselect = false;

            // Show the dialog and get the result
            Nullable<bool> result = openFileDialog.ShowDialog();

            // Process the result
            if (result == true)
            {
                bool isValid = true;
                bool isKOne = true;

                int countK = 0;
                // Get the selected file name
                string fileName = openFileDialog.FileName;

                // Read the contents of the file
                string fileContents = File.ReadAllText(fileName);

                int i = 0;
                string[] lines = fileContents.Split('\n');
                int rows = lines.Length;
                int cols = lines[0].Length-1;
                char[,] matrix = new char[rows, cols];

                for (i=0; i < rows; i++)
                {
                    for(int j=0;j<cols; j++)
                    {
                        matrix[i,j] = lines[i][j];
                        if (matrix[i,j] != 'T' && matrix[i, j] != 'R' && matrix[i, j] != 'K' && matrix[i, j] != 'X')
                        {
                            isValid = false;
                        }

                        if (matrix[i, j] == 'K')
                        {
                            countK++;
                            state.start = Tuple.Create(i, j);
                        }else if (matrix[i,j] == 'T')
                        {
                            state.TCounts++;
                        }
                    }
                }

                if (countK > 1 || countK < 1)
                {
                    isKOne = false;
                }

                if (isValid && isKOne)
                {
                    state.map = matrix;
                    state.visited = new bool[rows, cols];
                    createMatrix();
                }
                else
                {
                    if (!isKOne) {
                        errorMessageGrid("Pastikan Krusty Crab berjumlah satu");
                    }
                    else
                    {
                        errorMessageGrid("Pastikan file hanya berisi karakter T,R,X,K");
                    }
                }
            }
        }

        private void errorMessageGrid(string errMsg)
        {
            Grid grid = new Grid();

            RowDefinition rowDef = new RowDefinition();
            grid.RowDefinitions.Add(rowDef);

            TextBlock textBlock = new TextBlock();
            textBlock.Text = errMsg;

            grid.Children.Add(textBlock);

            Grid.SetRow(textBlock, 0);

            Grid mygrid = (Grid)FindName("myMap");
            mygrid.Children.Clear();
            mygrid.Children.Add(grid);
        }

        private void createMatrix()
        {
            int rows = state.map.GetLength(0);
            int cols = state.map.Length/rows;

            Grid grid = new Grid(); 
            
            for (int i = 0; i < rows; i++)
            {
                RowDefinition rowDef = new RowDefinition();
                grid.RowDefinitions.Add(rowDef);
            }

            for (int i = 0; i < cols; i++)
            {
                ColumnDefinition colDef = new ColumnDefinition();
                grid.ColumnDefinitions.Add(colDef);
            }

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {

                    TextBlock textBlock = new TextBlock();
                    if (state.map[i, j] == 'K')
                    {
                        textBlock.Text = "S";
                    }
                    else if (state.map[i, j] == 'T')
                    {
                        textBlock.Text = "T";
                    }
                    else if (state.map[i, j] == 'X')
                    {
                        textBlock.Background = Brushes.Black;
                    }
                    else
                    {
                        textBlock.Background = Brushes.White;
                    }

                    Tuple<int, int> tmp = new Tuple<int, int>(i, j);

                    if (state.path.Contains(tmp))
                    {
                        textBlock.Background = Brushes.Yellow;
                    }

                    if (state.currCheck != null)
                    {
                        if (state.currCheck.Item1 == i && state.currCheck.Item2 == j)
                        {
                            textBlock.Background = Brushes.Blue;
                        }
                    }

                    Border myBorder = new Border();
                    myBorder.BorderThickness = new Thickness(2);
                    myBorder.BorderBrush = Brushes.Black;
                    myBorder.Child = textBlock;

                    grid.Children.Add(myBorder);

                    Grid.SetRow(myBorder, i);
                    Grid.SetColumn(myBorder, j);
                }
            }


            grid.Width = 200;
            grid.Height = 200;

            Grid mygrid = (Grid)FindName("myMap");
            mygrid.Children.Clear();
            mygrid.Children.Add(grid);
        }

        private void search()
        {
            RadioButton toggleTSP = (RadioButton)FindName("radio_TSP");
            RadioButton radioBFS = (RadioButton)FindName("radio_BFS");
            RadioButton radioDFS = (RadioButton)FindName("radio_DFS");

            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();
            if (toggleTSP.IsChecked == true)
            {
                if(radioBFS.IsChecked == true)
                {
                    tspBFS();
                    createMatrix();
                }else if(radioDFS.IsChecked == true)
                {
                    tspDFS();
                    createMatrix();
                }
            }else
            {
                if (radioBFS.IsChecked == true)
                {
                    solveBFS();
                    createMatrix();
                }
                else if (radioDFS.IsChecked == true)
                {
                    solveDFS();
                    createMatrix();
                }
            }
            stopwatch.Stop();

            TextBlock textExc = (TextBlock)FindName("text_exc");
            textExc.Text += stopwatch.Elapsed.TotalMilliseconds.ToString() + " ms";

        }

        public bool goCheck(int x, int y) 
        {
            return x >= 0 && x < state.map.GetLength(0) && y >=0 && y < state.map.GetLength(1) && state.map[x,y] != 'X' && !state.visited[x,y];
        }

        public bool DFS(int x, int y, char target) 
        {
            // mark as visited
            state.visited[x, y] = true;
            state.visitBefore[x, y] = true;
            Tuple<int, int> temp = Tuple.Create(x, y);
            state.traversalPath.Add(temp);

            // add (x,y) to path
            if (state.path.Count == 0 || !state.path.Last().Equals(temp))
            {
                state.path.Add(temp);
            }

            // collect T if (x,y) is a treasure
            if (state.map[x, y] == target)
            {
                // change T value to R, inc TFounds, change current position
                state.map[x, y] = 'R';
                state.TFounds++;
                state.current = Tuple.Create(x,y);

                // return true
                return true;
            }
        

            // recursive
            var possibleDir = new List<Tuple<int, int>>();
            int cnt = 0;
            for (int i = 0; i < 4; i++) 
            {
                int x2 = x + state.dx[i];
                int y2 = y + state.dy[i];
                if (goCheck(x2, y2)) 
                {
                    if (!state.visitBefore[x2, y2])
                    {
                        cnt++;
                    }
                    state.anotherWay = state.anotherWay || !state.visitBefore[x2, y2];
                    possibleDir.Add(Tuple.Create(x2, y2));
                }
            }

            foreach (var dir in possibleDir)
            {
                int x2 = dir.Item1, y2 = dir.Item2;
                if ((!state.anotherWay && state.visitBefore[x2, y2]) 
                    || !state.visitBefore[x2, y2] || cnt == 0)
                {
                    if (DFS(x2, y2, target))
                    {
                        return true;
                    }
                }
            }

            // remove point from path
            state.path.RemoveAt(state.path.Count - 1);
            state.visited[x, y] = false;
            return false;
        }
    
        public void BFS(int x, int y, char target)
        {
            // make BFS queue then enque (x,y)
            Queue<Tuple<int, int>> queue = new Queue<Tuple<int, int>>();
            queue.Enqueue(Tuple.Create(x, y));

            // make a matrix of tuple as parent of each points
            state.parent = new Tuple<int, int>[state.map.GetLength(0), state.map.GetLength(1)];
            state.parent[x, y] = Tuple.Create(-1, -1);


            // loop while queue not empty
            while (queue.Count > 0) {
                // dequeue as temp then mark as visited
                Tuple<int, int> temp = queue.Dequeue();
                state.visited[temp.Item1, temp.Item2] = true;
            
                state.traversalPath.Add(temp);
                // Treasure encountered
                if (state.map[temp.Item1, temp.Item2] == target) {
                    // inc Counter, change current position, change map value to R
                    state.TFounds++;
                    state.current = Tuple.Create(temp.Item1, temp.Item2);
                    state.map[temp.Item1, temp.Item2] = 'R';

                    // List path from (x,y) to T then concat to path
                    List<Tuple<int, int>> tempPath = new List<Tuple<int, int>>();
                    while (!temp.Equals(new Tuple<int, int>(x, y))) {
                        tempPath.Add(temp);
                        temp = state.parent[temp.Item1, temp.Item2];
                        state.visitBefore[temp.Item1, temp.Item2] = true;
                    }
                    tempPath.Reverse();
                    state.path.AddRange(tempPath);
                
                    break;
                }

                // enqueue all possible road
                var possibleDir = new List<Tuple<int, int>>();
                for (int i = 3; i >= 0; i--) {
                    int x2 = temp.Item1 + state.dx[i];
                    int y2 = temp.Item2 + state.dy[i];

                    if (goCheck(x2, y2))
                    {
                        state.visited[x2, y2] = true;
                        
                        if (state.visitBefore[x2, y2])
                        {
                            possibleDir.Add(Tuple.Create(x2, y2));
                        }
                        else
                        {
                            possibleDir.Insert(0, Tuple.Create(x2, y2));
                        }
                    }
                }

                foreach (var dir in possibleDir)
                {   
                    int xd = dir.Item1, yd = dir.Item2;
                    state.parent[xd, yd] = temp;
                    queue.Enqueue(dir);
                }
            }
        }

        public void solveDFS()
        {
            state.current = state.start;
            state.visitBefore = new bool[state.map.GetLength(0), state.map.GetLength(1)];
            while (state.TFounds != state.TCounts)
            {
                state.anotherWay = false;
                state.visited = new bool[state.map.GetLength(0), state.map.GetLength(1)];
                DFS(state.current.Item1, state.current.Item2, 'T');
            }
            setRoute();
        }

        public void tspDFS()
        {
            state.current = state.start;
            state.visitBefore = new bool[state.map.GetLength(0), state.map.GetLength(1)];
            while (state.TFounds != state.TCounts)
            {
                state.anotherWay = false;
                state.visited = new bool[state.map.GetLength(0), state.map.GetLength(1)];
                DFS(state.current.Item1, state.current.Item2, 'T');
            }
            state.anotherWay = false;
            state.visited = new bool[state.map.GetLength(0), state.map.GetLength(1)];
            DFS(state.current.Item1, state.current.Item2, 'K');
            setRoute();
        }

        public void solveBFS()
        {
            state.current = state.start;
            state.visitBefore = new bool[state.map.GetLength(0), state.map.GetLength(1)];
            state.path.Add(state.start);
            state.visitBefore[state.start.Item1, state.start.Item2] = true;
            while (state.TFounds != state.TCounts)
            {
                state.visited = new bool[state.map.GetLength(0), state.map.GetLength(1)];
                BFS(state.current.Item1, state.current.Item2, 'T');
            }
            setRoute();
        }

        public void tspBFS()
        {
            state.current = state.start;
            state.visitBefore = new bool[state.map.GetLength(0), state.map.GetLength(1)];
            state.path.Add(state.start);
            state.visitBefore[state.start.Item1, state.start.Item2] = true;
            while (state.TFounds != state.TCounts)
            {
                state.visited = new bool[state.map.GetLength(0), state.map.GetLength(1)];
                BFS(state.current.Item1, state.current.Item2, 'T');
            }
            state.visited = new bool[state.map.GetLength(0), state.map.GetLength(1)];
            BFS(state.current.Item1, state.current.Item2, 'K');
            setRoute();
        }

        public void setRoute()
        {
            string route = "";
            Tuple<int, int> temp = null;
            foreach (var item in state.path)
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
            state.route = route;

            TextBlock mytext = (TextBlock)FindName("text_route");
            mytext.Text += " " + route;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            search();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {

            _currentIndex = 0;

            // Create the timer but don't start it yet
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += visualize;
            _timer.Start();
        }


        private void visualize(object sender, EventArgs e)
        {
            if(state.idxTraverse < state.traversalPath.Count)
            {
                state.currCheck = state.traversalPath[state.idxTraverse];
                createMatrix();
                state.idxTraverse++;
            }
            else
            {
                _timer.Stop();
            }
        }

    }
}
