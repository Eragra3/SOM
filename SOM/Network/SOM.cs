using System;
using System.Collections.Generic;
using MathNet.Numerics;
using MathNet.Numerics.Distributions;
using MathNet.Numerics.LinearAlgebra;

namespace SOM.Network
{
    public class SOM
    {
        public LinkedList<Neuron> Neurons { get; private set; }

        private double NeighbourhoodParam { get; set; }

        private double LearningRate { get; set; }
        private double LearningFunctionDecay { get; set; }

        public SOM(
            int neuronCount,
            int weightsCount,
            double initialWeightsRange,
            double neighbourhoodParam,
            double learningRate,
            double learningFunctionDecay)
        {
            NeighbourhoodParam = neighbourhoodParam;
            LearningRate = learningRate;
            LearningFunctionDecay = learningFunctionDecay;
            var distribution = new ContinuousUniform(-initialWeightsRange, initialWeightsRange);

            Neurons = new LinkedList<Neuron>();

            for (int i = 0; i < neuronCount; i++)
            {
                var neuron = new Neuron(weightsCount, distribution);
                Neurons.AddLast(neuron);
            }
        }

        public void TrainOneEpoch(IEnumerable<Vector<double>> inputs, double learningRate, int iteration)
        {
            LearningRate = learningRate;
            foreach (var input in inputs)
            {
                Neuron closestNeuron = null;
                var closestDist = double.PositiveInfinity;

                foreach (var neuron in Neurons)
                {
                    var distance = Distance.Euclidean(input, neuron.Weights);
                    if (closestDist > distance)
                    {
                        closestNeuron = neuron;
                        closestDist = distance;
                    }
                }

                closestNeuron.Learn(input, NeighbourhoodFunction(0, iteration), LearningFunction(iteration));

                bool end = false;
                var dist = 0;

                var winner = Neurons.Find(closestNeuron);
                var right = winner.NextOrFirst();
                var left = winner.PreviousOrLast();

                var visited = new HashSet<LinkedListNode<Neuron>> { winner };

                while (!end)
                {
                    dist++;
                    if (!visited.Contains(right))
                    {
                        right.Value.Learn(input, NeighbourhoodFunction(dist, iteration), LearningFunction(iteration));
                        visited.Add(right);
                        right = right.NextOrFirst();
                    }
                    else
                    {
                        end = true;
                    }
                    if (!visited.Contains(left))
                    {
                        left.Value.Learn(input, NeighbourhoodFunction(dist, iteration), LearningFunction(iteration));
                        visited.Add(left);
                        left = left.PreviousOrLast();
                    }
                    else
                    {
                        end = true;
                    }

                }
            }
        }

        private double NeighbourhoodFunction(double distance, int iteration)
        {
            double sigma = Math.Max(5 / Math.Sqrt(iteration), 0.1);
            var sigma2 = sigma * sigma;
            var scale = 1;
            var f = scale * (1 / Math.Sqrt(2 * sigma2 * Math.PI) * Math.Exp(-(distance * distance) / (2 * sigma2)));
            return Math.Min(f, 1);
        }

        private double LearningFunction(int iteration) => LearningRate;
        //private double LearningFunction(int iteration) => LearningRate * Math.Exp(-LearningFunctionDecay * iteration);
    }

    public static class LinkedList
    {
        public static LinkedListNode<T> NextOrFirst<T>(this LinkedListNode<T> current)
        {
            return current.Next ?? current.List.First;
        }

        public static LinkedListNode<T> PreviousOrLast<T>(this LinkedListNode<T> current)
        {
            return current.Previous ?? current.List.Last;
        }
    }
}
