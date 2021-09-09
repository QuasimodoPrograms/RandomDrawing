using System;
using System.Collections.Generic;
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

namespace RanDraw
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Line> lines = new List<Line>();

        class RectangleName
        {
            public Rectangle Rect { get; set; }
            public string Name { get; set; }
        }

        public MainWindow()
        {
            InitializeComponent();

            var values = typeof(Brushes).GetProperties().
                Select(p => new { Name = p.Name, Brush = p.GetValue(null) as Brush }).
                ToArray();
            var brushNames = values.Select(v => v.Name);

            List<RectangleName> rectangleNames = new List<RectangleName>();

            foreach(string brushName in brushNames)
            {
                RectangleName rn = new RectangleName { Rect = new Rectangle { Fill = new BrushConverter().ConvertFromString(brushName) as Brush },
                    Name = brushName };
                rectangleNames.Add(rn);
            }

            comboColor.ItemsSource = rectangleNames;
            comboColor.SelectedIndex = 7;

            comboBackgroundColor.ItemsSource = rectangleNames;
            comboBackgroundColor.SelectedIndex = comboBackgroundColor.Items.Count -8;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int x;
            if (Int32.TryParse(tb.Text, out x))
            {
                Random rnd = new Random();
                tb.ClearValue(TextBox.BackgroundProperty);
                for (int i = 0; i < x; i++)
                {
                    Line myLine;
                    myLine = new Line();
                    if (checkBox.IsChecked == true) comboColor.SelectedIndex = rnd.Next(comboColor.Items.Count);
                    myLine.Stroke = (SolidColorBrush)new BrushConverter().ConvertFromString((comboColor.SelectedItem as RectangleName).Name);

                    if (lines.Count == 0)
                    {
                        myLine.X1 = rnd.Next((int)canvas.ActualWidth);
                        myLine.X2 = rnd.Next((int)canvas.ActualWidth);
                        myLine.Y1 = rnd.Next((int)canvas.ActualHeight);
                        myLine.Y2 = rnd.Next((int)canvas.ActualHeight);
                    }
                    else
                    {
                        myLine.X1 = lines[lines.Count - 1].X2;
                        myLine.X2 = rnd.Next((int)canvas.ActualWidth);
                        myLine.Y1 = lines[lines.Count - 1].Y2;
                        myLine.Y2 = rnd.Next((int)canvas.ActualHeight);
                    }
                    if (checkBox_randomThickness.IsChecked == true) comboThickness.SelectedIndex = rnd.Next(comboThickness.Items.Count);
                    myLine.StrokeThickness = Convert.ToInt32( comboThickness.Text);

                    lines.Add(myLine);
                    canvas.Children.Add(myLine);
                }
            }
            else tb.Background = Brushes.Tomato;
        }

        private void btn_DeletePrevious_Click(object sender, RoutedEventArgs e)
        {
            if(lines.Count > 0)
            {
                lines.RemoveAt(lines.Count - 1);
                canvas.Children.RemoveAt(canvas.Children.Count - 1);
            }
        }

        private void btn_Clear_Click(object sender, RoutedEventArgs e)
        {
            lines.Clear();
            canvas.Children.Clear();
        }
    }
}