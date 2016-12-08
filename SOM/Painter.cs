using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;

namespace SOM
{
    public class Painter
    {
        private readonly double _centerX;
        private readonly double _centerY;

        public Painter(double centerX, double centerY)
        {
            _centerX = centerX;
            _centerY = centerY;
        }

        public Polyline GetSOMRepresentation(Network.SOM som)
        {
            var polyline = new Polyline();

            foreach (var neuron in som.Neurons)
            {
                var p = new Point(neuron.Weights[0], neuron.Weights[1]);
                p.Offset(_centerX, _centerY);
                polyline.Points.Add(p);
            }

            var first = som.Neurons.First.Value;
            var firstP = new Point(first.Weights[0], first.Weights[1]);
            firstP.Offset(_centerX, _centerY);
            polyline.Points.Add(firstP);

            return polyline;
        }
    }
}
