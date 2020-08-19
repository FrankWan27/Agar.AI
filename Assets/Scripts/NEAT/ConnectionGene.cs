using UnityEngine;
using System.Collections;
using System;

public class ConnectionGene
{
    public int inNode { get; set; }
    public int outNode { get; set; }
    public float weight { get; set; }
    public Boolean expressed { get; set; }
    public int innovation { get; set; }

    public ConnectionGene(int inNode, int outNode, float weight, Boolean expressed, int innovation)
    {
        this.inNode = inNode;
        this.outNode = outNode;
        this.weight = weight;
        this.expressed = expressed;
        this.innovation = innovation;
    }

    public ConnectionGene Clone()
    {
        return new ConnectionGene(inNode, outNode, weight, expressed, innovation);
    }



}
