using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Species : System.IComparable<Species>
{
    public Genome mascot { get; set; }
    public List<Genome> members;
    public float fitness { get; set; }

    public Species(Genome mascot)
    {
        this.mascot = mascot;
        members = new List<Genome>{mascot};
        fitness = 0;
    }

    public void AddMember(Genome g)
    {
        members.Add(g);
    }
    public int GetCount()
    {
        return members.Count;
    }

    public Genome GetRandomGenome()
    {
        return members[Random.Range(0, members.Count)];
    }

    public Genome GetBestGenome()
    {
        float championFitness = float.MinValue;
        Genome champion = null;
        foreach (Genome g in members)
        {
            if (g.nnet.fitness > championFitness)
            {
                championFitness = g.nnet.fitness;
                champion = g;
            }
        }
        return champion;
    }

    public void RandomizeMascot()
    {
        mascot = GetRandomGenome();
    }

    public void Reset()
    {
        members.Clear();
        fitness = 0;
    }

    public int CompareTo(Species other)
    {
        return other.fitness.CompareTo(fitness);
    }
}
