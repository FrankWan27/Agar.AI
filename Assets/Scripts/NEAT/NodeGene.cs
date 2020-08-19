using UnityEngine;
using System.Collections;

public class NodeGene
{
    public enum Type { Input, Hidden, Output };
    public Type type { get; set; }
    public GenomeUtils.Activation activation { get; set; }
    public int id { get; set; }

    public NodeGene(Type type, int id, GenomeUtils.Activation activation)
    {
        this.type = type;
        this.id = id;
        this.activation = activation;
    }

    public NodeGene(Type type, int id)
    {
        this.type = type;
        this.id = id;
        this.activation = GenomeUtils.RandomActivation();
    }

    public NodeGene Clone()
    {
        return new NodeGene(type, id, activation);
    }
 }
