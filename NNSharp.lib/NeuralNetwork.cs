using System;
using System.Collections.Generic;
namespace NNSharp.lib
{
    public class NeuralNetwork
    {
        public double LearningRate { get; set; }

        private int InputNodes { get; set; }

        private int HiddenNodes { get; set; }

        private int OutputNodes { get; set; }

        public Matrix WeightsOfInputHidden { get; set; }

        public Matrix WeightsOfHiddenOutput { get; set; }

        public Matrix WeighstOfBiasHidden { get; set; }

        public Matrix WeightsOfBiasOutput { get; set; }


        public NeuralNetwork(int inputLayer, int hiddenLayer, int outputLayer)
        {
            this.InputNodes = inputLayer;
            this.HiddenNodes = hiddenLayer;
            this.OutputNodes = outputLayer;

            this.WeightsOfInputHidden = new Matrix(this.HiddenNodes, this.InputNodes);
            this.WeightsOfHiddenOutput = new Matrix(this.OutputNodes, this.HiddenNodes);
            this.WeightsOfInputHidden.Randomize();
            this.WeightsOfHiddenOutput.Randomize();

            this.WeighstOfBiasHidden = new Matrix(this.HiddenNodes, 1);
            this.WeightsOfBiasOutput = new Matrix(this.OutputNodes, 1);
            this.WeighstOfBiasHidden.Randomize();
            this.WeightsOfBiasOutput.Randomize();
            this.LearningRate = 0.1;
        }

        public NeuralNetwork(NeuralNetwork network)
        {
            this.InputNodes = network.InputNodes;
            this.HiddenNodes = network.HiddenNodes;
            this.OutputNodes = network.OutputNodes;

            this.WeightsOfInputHidden = new Matrix(network.WeightsOfInputHidden);
            this.WeightsOfHiddenOutput = new Matrix(network.WeightsOfHiddenOutput);
            this.WeighstOfBiasHidden = new Matrix(network.WeighstOfBiasHidden);
            this.WeightsOfBiasOutput = new Matrix(network.WeightsOfBiasOutput);
            
            this.LearningRate = network.LearningRate;

        }

        public Matrix feedForward(Matrix input)
        {
            //Generating the hidden outputs
            Matrix hidden = Matrix.Multiply(this.WeightsOfInputHidden, input);
            hidden = Matrix.Add(hidden, this.WeighstOfBiasHidden);
            // Matrix.Print(hidden);
            map(hidden, sigmoidFunction);
            // Matrix.Print(hidden);

            Matrix output = Matrix.Multiply(this.WeightsOfHiddenOutput, hidden);
            output = Matrix.Add(output, this.WeightsOfBiasOutput);
            map(output, sigmoidFunction);
            // Matrix.Print(output);
            return output;

        }

        public void Train(Matrix inputs, Matrix targets)
        {
            //Feed forward
            Matrix hidden = Matrix.Multiply(this.WeightsOfInputHidden, inputs);
            hidden = Matrix.Add(hidden, this.WeighstOfBiasHidden);
            map(hidden, sigmoidFunction);
            Matrix outputs = Matrix.Multiply(this.WeightsOfHiddenOutput, hidden);
            outputs = Matrix.Add(outputs, this.WeightsOfBiasOutput);
            map(outputs, sigmoidFunction);

            //Calculate the output error
            Matrix output_errors = Matrix.Subtract(targets, outputs);

            Matrix gradients = outputs;
            map(gradients, gradientFunction);
            gradients = Matrix.HadamardProduct(gradients, output_errors);
            gradients = Matrix.Multiply(gradients, this.LearningRate);


            Matrix hidden_T = Matrix.Transpose(hidden); // hiddenden gelen output
            Matrix weight_ho_deltas = Matrix.Multiply(gradients, hidden_T);

            //Adjust the weights
            this.WeightsOfHiddenOutput = Matrix.Add(this.WeightsOfHiddenOutput, weight_ho_deltas);
            this.WeightsOfBiasOutput = Matrix.Add(this.WeightsOfBiasOutput, gradients);


            //Calculate the hidden layer error
            Matrix who_t = Matrix.Transpose(this.WeightsOfHiddenOutput);
            Matrix hidden_errors = Matrix.Multiply(who_t, output_errors);

            Matrix hidden_gradient = hidden;
            map(hidden, gradientFunction);
            hidden_gradient = Matrix.HadamardProduct(hidden_gradient, hidden_errors);
            hidden_gradient = Matrix.Multiply(hidden_gradient, this.LearningRate);


            //Calculate input-> hidden delta
            Matrix inputs_T = Matrix.Transpose(inputs);
            Matrix weight_ih_deltas = Matrix.Multiply(hidden_gradient, inputs_T);

            //Adjust the weights
            this.WeightsOfInputHidden = Matrix.Add(this.WeightsOfInputHidden, weight_ih_deltas);
            this.WeighstOfBiasHidden = Matrix.Add(this.WeighstOfBiasHidden, hidden_gradient);
        }

        public delegate double Func(double x);
        private double sigmoidFunction(double x)
        {
            return 1 / (1 + Math.Pow(Math.E, -x));
        }

        private double gradientFunction(double x)
        {
            return x * (1 - x);
        }

        public void map(Matrix matrix, Func function)
        {
            for (int i = 0; i < matrix.Rows; i++)
            {
                for (int j = 0; j < matrix.Columns; j++)
                {
                    matrix.Array[i, j] = function(matrix.Array[i, j]);
                }
            }
        }

        public void Save()
        {
            //To-do
        }


    }
}
