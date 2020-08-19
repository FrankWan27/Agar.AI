using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class GenomeUtils
{
    public static float WEIGHT_MIN = -1f;
    public static float WEIGHT_MAX = 1f;
    public static float ASEXUAL_RATE = 0.25f;
    public static float PERTURB_RATE = 0.9f;
    public static float MUTATION_RATE = 0.8f;
    public static float ADD_NODE_RATE = 0.03f;
    public static float ADD_CONNECTION_RATE = 0.05f;
    public static float SPECIES_DIST = 3f;
    public enum Activation { ReLU, Tanh, Sigmoid, ELU, LeLU, None };
    public static System.Array activations = System.Enum.GetValues(typeof(Activation));
    public static Activation RandomActivation()
    {
        return (Activation)Random.Range(0, activations.Length);
    }
    public static float Activate(float val, Activation activation)
    {
        switch(activation)
        {
            case Activation.ReLU:
                return ReLU(val);
            case Activation.Tanh:
                return Tanh(val);
            case Activation.Sigmoid:
                return Sigmoid(val);
            case Activation.ELU:
                return ELU(val);
            case Activation.LeLU:
                return LeLU(val);
            case Activation.None:
                return val;
        }
        Debug.LogError("Invalid Activation Function");
        return val;
    }
    public static float ReLU(float val)
    {
        return Mathf.Max(0, val);
    }
    public static float Tanh(float val)
    {
        return (float)System.Math.Tanh((double)val);
    }
    public static float Sigmoid(float val)
    {
        return 1f / (1f + Mathf.Exp(-val));
    }
    public static float ELU(float val)
    {
        return Mathf.Max(0.01f * (Mathf.Exp(val) - 1), val);
    }
    public static float LeLU(float val)
    {
        return Mathf.Max(0.01f * val, val);
    }

    public static void PrintGenome(Genome genome)
    {
        Dictionary<int, NodeGene> nodes = genome.GetNodeGenes();
        Dictionary<int, ConnectionGene> connections = genome.GetConnectionGenes();

        Debug.Log("Nodes:");
        foreach (int key in nodes.Keys)
        {
            Debug.Log("id: " + key + " type: " + nodes[key].type);
        }

        Debug.Log("Connections:");
        foreach (int key in connections.Keys)
        {
            Debug.Log("innovation: " + key + " in: " + connections[key].inNode + " out: " + connections[key].outNode + " weight: " + connections[key].weight + " expressed: " + connections[key].expressed);
        }



    }
    public static float RandomWeight()
    {
        return Random.Range(WEIGHT_MIN, WEIGHT_MAX);
    }
    public static Genome Crossover(Genome parent1, Genome parent2)
    {
        Genome child = new Genome();
        foreach (NodeGene parent1Node in parent1.GetNodeGenes().Values)
        {
            child.AddNodeGene(parent1Node.Clone());
        }

        foreach (ConnectionGene parent1Node in parent1.GetConnectionGenes().Values)
        {
            if (parent2.GetConnectionGenes().ContainsKey(parent1Node.innovation)) //matching gene
            {
                child.AddConnectionGene(Random.value < 0.5f ? parent1Node.Clone() : parent2.GetConnectionGenes()[parent1Node.innovation].Clone());
            }
            else
            {
                child.AddConnectionGene(parent1Node.Clone());
            }
        }

        return child;
    }

    public static Genome Clone(Genome genome)
    {
        Genome child = new Genome();
        foreach (NodeGene node in genome.GetNodeGenes().Values)
            child.AddNodeGene(node.Clone());

        foreach (ConnectionGene con in genome.GetConnectionGenes().Values)
            child.AddConnectionGene(con.Clone());

        return child;
    }

    public static float CompatibilityDistance(Genome genome1, Genome genome2, float c1, float c2, float c3)
    {
        int excessGenes = 0;
        int disjointGenes = 0;
        int matchingGenes = 0;

        float weightDifference = 0;

        //connections
        int genome1Max = genome1.GetMaxConnection();
        int genome2Max = genome2.GetMaxConnection();

        int indices = Mathf.Max(genome1Max, genome2Max);

        for (int i = 1; i <= indices; i++)
        {
            ConnectionGene connection1;
            ConnectionGene connection2;
            if (genome1.GetConnectionGenes().TryGetValue(i, out connection1))
            {
                if (genome2.GetConnectionGenes().TryGetValue(i, out connection2))
                {
                    matchingGenes++;
                    weightDifference += Mathf.Abs(connection1.weight - connection2.weight);
                }
                else if (genome2Max < i)
                    excessGenes++;
                else
                    disjointGenes++;
            }
            else if (genome2.GetConnectionGenes().TryGetValue(i, out connection2))
            {
                if (genome1Max < i)
                    excessGenes++;
                else
                    disjointGenes++;
            }
        }

        int n = Mathf.Max(genome1.GetNodeGenes().Count, genome2.GetNodeGenes().Count);

        if (n < 20)
            n = 1;

        return (excessGenes * c1) / n + (disjointGenes * c2) / n + (weightDifference / matchingGenes) * c3;
    }
}
