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
using MathNet.Numerics.Distributions;

namespace SOM
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public const int POINT_RADIUS = 2;

        public double InitialWeightsRange { get; set; }

        public IList<City> Cities { get; } = new List<City>();

        private Network.SOM _som;
        private Painter _painter;
        private IContinuousDistribution _rng;

        public MainWindow()
        {
            InitializeComponent();

            ContentRendered += (sender, args) =>
            {
                _painter = new Painter(World.ActualWidth / 2.0, World.ActualHeight / 2.0);

                InitialWeightsRange = World.ActualWidth / 5.0;

                _rng = new ContinuousUniform(-World.ActualWidth / 4.0, World.ActualWidth / 4.0);

                GenerateRandomCities(10);
                CreateNewSOM();
                Redraw();
            };
        }

        private void CreateNewSOM()
        {
            _som = new Network.SOM(Cities.Count, 2, InitialWeightsRange);
        }

        private void Redraw()
        {
            World.Children.Clear();
            DrawCities();
            DrawSOM();
        }

        private void GenerateRandomCities(int count)
        {
            Cities.Clear();

            for (int i = 0; i < count; i++)
            {
                var point = new Point(_rng.Sample(), _rng.Sample());

                var city = new City
                {
                    Name = "Random",
                    Point = point
                };

                Cities.Add(city);
            }
        }

        private void DrawSOM(object sender, RoutedEventArgs e)
        {
            Redraw();
        }

        private void GenerateSOM(object sender, RoutedEventArgs e)
        {
            CreateNewSOM();
        }

        #region HELPER DRAW METHODS

        private void DrawSOM()
        {
            var somRepresentation = _painter.GetSOMRepresentation(_som);

            somRepresentation.Stroke = Brushes.Blue;

            World.Children.Add(somRepresentation);
        }

        private void DrawCities()
        {
            foreach (var city in Cities)
            {
                var ellipse = new Ellipse()
                {
                    Width = POINT_RADIUS,
                    Height = POINT_RADIUS
                };

                ellipse.Fill = Brushes.Black;
                Canvas.SetLeft(ellipse, city.Point.X)
                    ;
                Canvas.SetTop(ellipse, city.Point.Y);

                World.Children.Add(ellipse);
            }
        }
        #endregion
    }
}
