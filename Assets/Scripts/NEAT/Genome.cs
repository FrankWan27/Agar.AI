using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Genome : System.IComparable<Genome>
{
    Dictionary<int, ConnectionGene> connections;
    Dictionary<int, NodeGene> nodes;
    public NNet nnet;

    public Genome()
    {
        nodes = new Dictionary<int, NodeGene>();
        connections = new Dictionary<int, ConnectionGene>();
    }
    public Genome(int inputNodes, int outputNodes)
    {
        Counter.Reset();

        nodes = new Dictionary<int, NodeGene>();
        connections = new Dictionary<int, ConnectionGene>();

        for (int i = 0; i < inputNodes; i++)
            AddNodeGene(new NodeGene(NodeGene.Type.Input, Counter.NextNode(), GenomeUtils.Activation.None));

        for(int j = inputNodes + 1; j <= inputNodes + outputNodes; j++)
        {
            AddNodeGene(new NodeGene(NodeGene.Type.Output, Counter.NextNode(), GenomeUtils.Activation.None));
            for(int i = 1; i <= inputNodes; i++)
            {
                float weight = GenomeUtils.RandomWeight();
                AddConnectionGene(new ConnectionGene(i, j, weight, true, Counter.NextConnection()));
            }
        }
    }

    public Dictionary<int, ConnectionGene> GetConnectionGenes()
    {
        return connections;
    }
    public Dictionary<int, NodeGene> GetNodeGenes()
    {
        return nodes;
    }
    public void AddNodeGene(NodeGene gene)
    {
        nodes.Add(gene.id, gene);
    }
    public void AddConnectionGene(ConnectionGene gene)
    {
        connections.Add(gene.innovation, gene);
    }
    public int GetMaxNode()
    {
        return Mathf.Max(nodes.Keys.ToArray());
    }
    public int GetMaxConnection()
    {
        return Mathf.Max(connections.Keys.ToArray());
    }

    public void Mutate()
    {
        foreach(ConnectionGene con in connections.Values)
        {
            if (Random.value < GenomeUtils.PERTURB_RATE) //TODO CHANGE TO NORMAL DISTRIBUTION
            {
                con.weight = con.weight += Random.Range(GenomeUtils.WEIGHT_MIN / 2, GenomeUtils.WEIGHT_MAX / 2);
                //con.weight = Mathf.Clamp(con.weight, GenomeUtils.WEIGHT_MIN, GenomeUtils.WEIGHT_MAX);
            }
            else
                con.weight = GenomeUtils.RandomWeight();
        }
    }

    //Add a random connection between two nodes
    public void AddConnectionMutation()
    {
        List<NodeGene> values = Enumerable.ToList(nodes.Values);
        NodeGene node1 = values[Random.Range(0, values.Count)];
        NodeGene node2 = values[Random.Range(0, values.Count)];

        if (node1.type == node2.type && node1.type != NodeGene.Type.Hidden)
        {
            //try again
            AddConnectionMutation();
            return;
        }

        bool reversed = false;
        if(node1.type == NodeGene.Type.Hidden && node2.type == NodeGene.Type.Input)
        {
            reversed = true;
        }
        else if(node1.type == NodeGene.Type.Output && node2.type == NodeGene.Type.Input)
        {
            reversed = true;
        }
        else if(node1.type == NodeGene.Type.Output && node2.type == NodeGene.Type.Hidden)
        {
            reversed = true;
        }


        float weight = GenomeUtils.RandomWeight();

        bool connectionExists = false;
        
        foreach(ConnectionGene con in connections.Values)
        {
            if(con.inNode == node1.id && con.outNode == node2.id || con.inNode == node2.id && con.outNode == node1.id)
            {
                connectionExists = true;
                break;
            }
        }

        if (connectionExists)
            return;

        ConnectionGene newCon = new ConnectionGene(reversed? node2.id : node1.id, reversed? node1.id : node2.id, weight, true, Counter.NextConnection());
        connections.Add(newCon.innovation, newCon);
    }

    //Split a random connection into 2 connections
    public void AddNodeMutation()
    {
        List<ConnectionGene> values = Enumerable.ToList(connections.Values);
        ConnectionGene con = values[Random.Range(0, values.Count)];

        NodeGene inNode = nodes[con.inNode];
        NodeGene outNode = nodes[con.outNode];



        con.expressed = false;

        NodeGene newNode = new NodeGene(NodeGene.Type.Hidden, Counter.NextNode());

        ConnectionGene inToNew = new ConnectionGene(inNode.id, newNode.id, 1f, true, Counter.NextConnection());
        ConnectionGene newToOut = new ConnectionGene(newNode.id, outNode.id, con.weight, true, Counter.NextConnection());

        nodes.Add(newNode.id, newNode);
        connections.Add(inToNew.innovation, inToNew);
        connections.Add(newToOut.innovation, newToOut);
    }

    public int CompareTo(Genome other)
    {
        return nnet.CompareTo(other.nnet);
    }
}
