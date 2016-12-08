using MathNet.Numerics.Distributions;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace SOM
{
    public class Neuron
    {
        public static IContinuousDistribution ContinuousDistribution { get; set; }

        public Vector<double> Weights { get; set; }

        public Neuron(int weightsCount)
        {
            Weights = DenseVector.CreateRandom(weightsCount, ContinuousDistribution);
        }
    }
}
