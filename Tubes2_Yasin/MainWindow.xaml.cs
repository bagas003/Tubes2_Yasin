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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string map;

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
                    }
                }
                createMatrix(matrix);
            }
        }

        private void createMatrix(char[,] matrix)
        {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(0);

            Grid grid = new Grid();


            for (int i = 0; i < rows; i++)
            {
                RowDefinition rowDef = new RowDefinition();
                grid.RowDefinitions.Add(rowDef);

                for (int j = 0; j < cols; j++)
                {
                    ColumnDefinition colDef = new ColumnDefinition();
                    grid.ColumnDefinitions.Add(colDef);

                    DataContext = new Output { column = rows, row = cols, isi = matrix[0,0] };

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

                    textBlock.Width = 600/cols;
                    textBlock.Height = 600/rows;

                    grid.Children.Add(myBorder);

                    Grid.SetRow(myBorder, i);
                    Grid.SetColumn(myBorder, j);

                    myBorder.HorizontalAlignment = HorizontalAlignment.Stretch;
                    myBorder.VerticalAlignment = VerticalAlignment.Stretch;

                    myBorder.Width = 400/cols;
                    myBorder.Height = 400/rows;

                }
            }
            grid.Width = 400;
            grid.Height = 400;

            Grid mygrid = (Grid)FindName("myMap");
            mygrid.Children.Add(grid);
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void RadioButton_Checked_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
