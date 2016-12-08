using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.Distributions;

namespace SOM
{
    public class SOM
    {
        public LinkedList<Neuron> Neurons { get; set; }

        public SOM(int neuronCount, int weightsCount, double initialWeightsRange)
        {
            Neuron.ContinuousDistribution = new ContinuousUniform(-initialWeightsRange, initialWeightsRange);

            for (int i = 0; i < neuronCount; i++)
            {
                var neuron = new Neuron(weightsCount);
                Neurons.AddLast(neuron);
            }
        }
    }
}
