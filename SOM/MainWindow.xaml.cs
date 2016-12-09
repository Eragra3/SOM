using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using MathNet.Numerics.Distributions;
using MathNet.Numerics.LinearAlgebra.Double;

namespace SOM
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public const int POINT_RADIUS = 5;

        public double InitialWeightsRange { get; set; }
        public double NeighbourhoodParam { get; set; }
        public double LearningRate { get; set; }
        public double LearningRateDecay { get; set; }
        public int CitiesCount { get; set; }

        public IList<City> Cities { get; } = new List<City>();

        private Network.SOM _som;
        private Painter _painter;
        private IContinuousDistribution _rng;

        private int _currentIteration;

        private DispatcherTimer _timer;

        public MainWindow()
        {
            InitializeComponent();

            ContentRendered += (sender, args) =>
            {
                _painter = new Painter(World.ActualWidth / 2.0, World.ActualHeight / 2.0);
                _timer = new DispatcherTimer(DispatcherPriority.Send);
                _timer.Interval = TimeSpan.FromMilliseconds(50);

                InitialWeightsRange = World.ActualWidth / 6.0;
                NeighbourhoodParam = 0.2;
                LearningRate = 0.5;
                LearningRateDecay = 0.04;

                CitiesCount = 5;

                _rng = new ContinuousUniform(-World.ActualWidth / 5.0, World.ActualWidth / 5.0);

                GenerateRandomCities(CitiesCount);
                CreateNewSOM();
                Redraw();
            };
        }

        private void CreateNewSOM()
        {
            _som = new Network.SOM(Cities.Count, 2, InitialWeightsRange, NeighbourhoodParam, LearningRate, LearningRateDecay);
            _currentIteration = 0;
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
            Redraw();
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
                var ellipse = new Ellipse
                {
                    Width = POINT_RADIUS,
                    Height = POINT_RADIUS,
                    Fill = Brushes.Black
                };

                var worldPoint = _painter.LocalToWorld(city.Point);

                var x = worldPoint.X;
                var y = worldPoint.Y;

                Canvas.SetLeft(ellipse, x);
                Canvas.SetTop(ellipse, y);

                World.Children.Add(ellipse);
            }
        }

        private void TrainOneEpoch()
        {
            var input = Cities.Select(c => new DenseVector(2) { [0] = c.Point.X, [1] = c.Point.Y });
            _som.TrainOneEpoch(input, LearningRate, _currentIteration++);
        }
        #endregion

        private void TrainOneEpoch(object sender, RoutedEventArgs e)
        {
            TrainOneEpoch();
            Redraw();
        }

        private void GenerateCities(object sender, RoutedEventArgs e)
        {
            GenerateRandomCities(CitiesCount);
            Redraw();
        }

        private void Start(object sender, RoutedEventArgs e)
        {
            _timer.Tick += TrainOneEpoch;
            _timer.Start();
        }

        private void TrainOneEpoch(object sender, EventArgs e)
        {
            TrainOneEpoch();
            Redraw();
        }

        private void Stop(object sender, RoutedEventArgs e)
        {
            _timer.Tick -= TrainOneEpoch;
            _timer.Stop();
        }
    }
}
