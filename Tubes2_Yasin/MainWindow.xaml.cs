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
            DataContext = new Person { Name = "eruivfhieruhfu", Age = 20, PhoneNumber = "123456" };
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

                string[] str = fileContents.Split('\n');
                createMatrix(str);
            }
        }

        private void createMatrix(string[] str)
        {
            int rows = str.Length;
            int cols = str.Length;

            Grid grid = new Grid();

            for (int i = 0; i < rows; i++)
            {
                RowDefinition rowDef = new RowDefinition();
                grid.RowDefinitions.Add(rowDef);

                for (int j = 0; j < cols; j++)
                {
                    ColumnDefinition colDef = new ColumnDefinition();
                    grid.ColumnDefinitions.Add(colDef);


                    TextBlock textBlock = new TextBlock();
                    if (str[i][j] == '1')
                    {
                        textBlock.Background = Brushes.Black;
                    }

                    Grid.SetRow(textBlock, i);
                    Grid.SetColumn(textBlock, j);

                    grid.Children.Add(textBlock);
                }
            }

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
