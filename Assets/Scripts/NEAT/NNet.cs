using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class NNet : IComparable<NNet>
{
    public Genome genome;
    Dictionary<int, Node> nodes;
    public List<Node> inputNodes;
    public List<Node> hiddenNodes;
    public List<Node> outputNodes;
    public List<Connection> connections;
    public float fitness;

    public NNet(Genome genome)
    {
        this.genome = genome;
        genome.nnet = this;
        nodes = new Dictionary<int, Node>();
        inputNodes = new List<Node>();
        hiddenNodes = new List<Node>();
        outputNodes = new List<Node>();
        connections = new List<Connection>();

        Initialize();
    }

    void Initialize()
    {
        foreach(NodeGene nodeGene in genome.GetNodeGenes().Values)
        {
            Node newNode = new Node(nodeGene);
            switch (nodeGene.type)
            {
                case NodeGene.Type.Input:
                    inputNodes.Add(newNode);
                    break;
                case NodeGene.Type.Hidden:
                    hiddenNodes.Add(newNode);
                    break;
                case NodeGene.Type.Output:
                    outputNodes.Add(newNode);
                    break;
            }
            nodes.Add(nodeGene.id, newNode);
        }

        foreach(ConnectionGene con in genome.GetConnectionGenes().Values)
        {
            if (con.expressed)
            {
                Connection newCon = new Connection(con, genome.GetNodeGenes()[con.inNode].type, genome.GetNodeGenes()[con.outNode].type);
                nodes[con.inNode].AddConnection(newCon, Node.TYPE.OUT);
                nodes[con.outNode].AddConnection(newCon, Node.TYPE.IN);
                connections.Add(newCon);
            }
        }
    }

    void PrintConnections()
    {
        foreach(Connection con in connections)
        {
            Debug.Log(con.inNode + " to " + con.outNode + " value: " + con.value);
        }
    }

    void ProcessNodes(List<Node> nodesLeft)
    {
        //Debug.Log("Start hidden");

        while (nodesLeft.Count > 0)
        {
            List<Node> toRemove = new List<Node>();
            foreach (Node node in nodesLeft)
            {
                if (node.Ready())
                {
                    node.ReceiveValue();
                    node.TransmitValue();
                    toRemove.Add(node);
                }
            }
            foreach (Node node in toRemove)
            {
                nodesLeft.Remove(node);
            }
        }
        //Debug.Log("End hidden");

    }

    public float[] GetOutput(float[] input)
    {
        //Debug.Log(string.Join(", ", input));
        float[] output = new float[outputNodes.Count];

        for(int i = 0; i < inputNodes.Count; i++)
        {
            inputNodes[i].value = input[i];
            inputNodes[i].TransmitValue();
        }

        List<Node> nodesLeft = new List<Node>(hiddenNodes);

        ProcessNodes(nodesLeft);


        for(int i = 0; i < outputNodes.Count; i++)
        {
            outputNodes[i].ReceiveValue();
            output[i] = outputNodes[i].value;
        }

        return output;
    }

    public int CompareTo(NNet other)
    {
        return other.fitness.CompareTo(fitness);
    }

    public class Node
    {
        public enum TYPE { IN, OUT }
        public GenomeUtils.Activation activation { get; set; }
        public int id { get; set; }
        public float value { get; set; }
        public NodeGene.Type type;
        List<Connection> inConnections;
        List<Connection> outConnections;

        public Node(NodeGene nodeGene)
        {
            this.id = nodeGene.id;
            this.activation = nodeGene.activation;
            this.type = nodeGene.type;
            value = 0f;
            inConnections = new List<Connection>();
            outConnections = new List<Connection>();
        }

        public void AddConnection(Connection connection, TYPE type)
        {
            if (type == TYPE.IN)
                inConnections.Add(connection);
            else if (type == TYPE.OUT)
                outConnections.Add(connection);
        }

        public void ReceiveValue()
        {
            value = 0;
            if (activation == GenomeUtils.Activation.Multiply)
                value = 1;
            foreach (Connection con in inConnections)
            {
                if (activation == GenomeUtils.Activation.Multiply)
                    value *= con.value * con.weight;
                else
                    value += con.value * con.weight;
                con.Reset();
            }
            value = GenomeUtils.Activate(value, activation);
        }

        public void TransmitValue()
        {
            foreach(Connection con in outConnections)
            {
                con.value = value;
                con.Set();
            }
        }

        public bool Ready()
        {
            bool ready = true;
            foreach(Connection con in inConnections)
            {
                if(!con.ready && !con.recurrent)
                {
                    ready = false;
                    break;
                }
            }
            return ready;
        }
    }

    public class Connection
    {
        public int inNode;
        public int outNode;
        public float value { get; set; }
        public float weight { get; set; }
        public bool ready { get; set; }
        public bool recurrent { get; set; }

        public Connection(ConnectionGene con, NodeGene.Type inType, NodeGene.Type outType)
        {
            inNode = con.inNode;
            outNode = con.outNode;
            value = 0f;
            ready = false;
            weight = con.weight;
            recurrent = false;
            if (inType == NodeGene.Type.Output && (outType == NodeGene.Type.Input || outType == NodeGene.Type.Hidden))
                recurrent = true;
            else if (inType == NodeGene.Type.Hidden && outType == NodeGene.Type.Input)
                recurrent = true;
            else if (inType == outType)
                recurrent = true;
        }

        public void Reset()
        {
            ready = false;
            value = 0f;
        }

        public void Set()
        {
            ready = true;
        }
    }
}
