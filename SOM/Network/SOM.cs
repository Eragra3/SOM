using System.Collections.Generic;
using MathNet.Numerics.Distributions;

namespace SOM.Network
{
    public class SOM
    {
        public LinkedList<Neuron> Neurons { get; private set; }

        public SOM(int neuronCount, int weightsCount, double initialWeightsRange)
        {
            Neurons = new LinkedList<Neuron>();

            var distribution = new ContinuousUniform(-initialWeightsRange, initialWeightsRange);

            for (int i = 0; i < neuronCount; i++)
            {
                var neuron = new Neuron(weightsCount, distribution);
                Neurons.AddLast(neuron);
            }
        }
    }
}
