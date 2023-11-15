# NeuralNetworkLife
a visual implementation of neuralnetwork from scratch in c# window form, genetic algoritm unsupervisioned learning

Simple explanation
This C# code defines a simple neural network (NN) implementation along with an agent that uses this neural network for decision-making. Here's a breakdown of the key components:

Namespace and Class Structure:

The code is organized into the NN_try_1 namespace, which contains various classes related to neural networks.
The main class is brain, representing the neural network.
Neural Network Structure:

The brain class has lists for input, output, and two hidden layers of nodes.
Nodes are the basic units of the neural network, and they can be of different types: input, output, or hidden (generic).
Nodes have weights and biases, which are used for signal propagation in the network.
Node Activation:

The activate method in the node class processes the input, considering weights, biases, and activation functions.
The signaling method in the neuron class applies the Rectified Linear Unit (ReLU) activation function.
Neural Network Initialization:

The neural network is initialized with random weights and biases.
The Mutate method introduces random mutations to the network based on a specified percentage.
Neural Network Combination:

The + operator is overloaded to combine two neural networks, creating a new one with a mix of their properties.
Agent Class:

The agent class represents an entity that uses the neural network for decision-making.
It has a brain object and methods to reset its position, mutate the brain, and perform actions based on neural network outputs.
Agent Movement and Decision-making:

The agent has a position and a direction it is looking at (LookAt).
It can move forward (AiForward) and turn (AiTurn) based on the neural network outputs.
The fitness value is updated based on the agent's interactions with the environment, and it influences the agent's behavior.
Other Functionalities:

The code includes some additional functionalities, such as resetting the agent's position, mutating the brain, and handling food interactions.
Graphics and Visualization:

There's a basic graphics component for visualizing the agent's movement and interactions with food.
In summary, the code provides a simple neural network implementation along with an agent that uses this network for decision-making in a simulated environment. The neural network learns and evolves over time through mutations and interactions with the environment.
