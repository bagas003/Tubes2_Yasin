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

namespace Tubes2_Yasin
{
    public partial class MainWindow : Window
    {
        public string route { get; set; }
        public int nodes { get; set; }
        public int steps { get; set; }
        public int time { get; set; }

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
                // Get the selected file name
                string fileName = openFileDialog.FileName;

                // Read the contents of the file
                string fileContents = File.ReadAllText(fileName);

                int i = 0;
                string[] lines = fileContents.Split('\n');
                int length = lines.Length;
                char[,] matrix = new char[length, length];
                for(i=0; i < length; i++)
                {
                    for(int j=0;j<length; j++)
                    {
                        matrix[i,j] = lines[i][j];
                        if (matrix[i,j] != 'T' && matrix[i, j] != 'R' && matrix[i, j] != 'K' && matrix[i, j] != 'X')
                        {
                            isValid = false;
                        }
                    }
                }
                if (isValid)
                {
                    createMatrix(matrix);
                }
                else
                {
                    Grid grid = new Grid();

                    RowDefinition rowDef = new RowDefinition();
                    grid.RowDefinitions.Add(rowDef);

                    TextBlock textBlock = new TextBlock();
                    textBlock.Text = "File tidak valid";

                    grid.Children.Add(textBlock);

                    Grid.SetRow(textBlock, 0);

                    Grid mygrid = (Grid)FindName("myMap");
                    mygrid.Children.Add(grid);
                }
            }
        }

        private void createMatrix(char[,] matrix)
        {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(0);

            Grid grid = new Grid();

            for(int i = 0; i < rows; i++)
            {
                RowDefinition rowDef = new RowDefinition();
                grid.RowDefinitions.Add(rowDef);
            }

            for (int i = 0; i < rows; i++)
            {
                ColumnDefinition colDef = new ColumnDefinition();
                grid.ColumnDefinitions.Add(colDef);
            }

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {

                    TextBlock textBlock = new TextBlock();
                    if (matrix[i, j] == 'K')
                    {
                        textBlock.Text = "S";
                    }
                    else if (matrix[i, j] == 'T')
                    {
                        textBlock.Text = "T";
                    }
                    else if (matrix[i, j] == 'X')
                    {
                        textBlock.Background = Brushes.Black;
                    }
                    else
                    {
                        textBlock.Background = Brushes.White;
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
            mygrid.Children.Add(grid);
        }

        private void search(object sender, TextChangedEventArgs e)
        {
            RadioButton radioTSP = (RadioButton)FindName("radio_TSP");
            RadioButton radioDFS = (RadioButton)FindName("radio_DFS");
            RadioButton radioBFS = (RadioButton)FindName("radio_BFS");
            RadioButton radioBiasa = (RadioButton)FindName("radio_biasa");

            if (radioTSP.IsChecked == true)
            {
                if(radioBFS.IsChecked == true)
                {

                }else if(radioDFS.IsChecked == true)
                {

                }
            }else if(radioBiasa.IsChecked == true)
            {
                if (radioBFS.IsChecked == true)
                {

                }
                else if (radioDFS.IsChecked == true)
                {

                }
            }
        }
    }
}
