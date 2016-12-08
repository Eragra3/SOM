using MathNet.Numerics.Distributions;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace SOM.Network
{
    public class Neuron
    {
        public Vector<double> Weights { get; set; }

        public Neuron(int weightsCount, IContinuousDistribution distribution)
        {
            Weights = DenseVector.CreateRandom(weightsCount, distribution);
        }
    }
}
