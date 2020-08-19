using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class NEATManager : MonoBehaviour
{
    public int generation;
    private int population;
    public int inputNodes;
    public int outputNodes;
    public float C1 = 1f;
    public float C2 = 1f;
    public float C3 = 0.3f;
    List<Genome> genomes;
    List<Species> species;
    List<NNet> nnets;
    List<NNet> unusedNNets;
    Dictionary<Genome, Species> speciesMap;

    void Start()
    {
        generation = 1;
        population = Utils.POP_SIZE;
        genomes = new List<Genome>();
        for(int i = 0; i < population; i++)
        {
            Genome genome = new Genome(inputNodes, outputNodes);
            genomes.Add(genome);
        }
        SetSpecies();
        MakeNNets();
    }

    void MakeNNets()
    {
        unusedNNets = new List<NNet>();
        nnets = new List<NNet>();
        foreach(Genome g in genomes)
        {
            NNet nnet = new NNet(g);
            unusedNNets.Add(nnet);
            nnets.Add(nnet);
        }
    }

    public NNet GetNNet()
    {
        if(unusedNNets.Count <= 0)
        {
            Debug.LogError("No NNets Remaining");
            return null;
        }
        NNet unusedNNet = unusedNNets[0];
        unusedNNets.RemoveAt(0);
        return unusedNNet;
    }

    void SetSpecies()
    {
        species = new List<Species>();
        speciesMap = new Dictionary<Genome, Species>();
        foreach (Genome g in genomes)
        {
            bool match = false;
            foreach (Species s in species)
            {
                if (GenomeUtils.CompatibilityDistance(g, s.mascot, C1, C2, C3) < GenomeUtils.SPECIES_DIST)
                {
                    s.AddMember(g);
                    speciesMap.Add(g, s);
                    match = true;
                    break;
                }
            }
            if (!match)
            {
                Species newSpecies = new Species(g);
                species.Add(newSpecies);
                speciesMap.Add(g, newSpecies);
            }
        }


        Debug.Log("Gen: " + generation + ", Population: " + population + ", Species: " + species.Count);

    }

    public void CalculateFitness()
    {
        foreach(NNet nnet in nnets)
        {
            nnet.fitness = nnet.fitness / speciesMap[nnet.genome].GetCount();
            speciesMap[nnet.genome].fitness += nnet.fitness;
        }

        nnets.Sort();
        species.Sort();

    }
    public void NextGeneration()
    {
        CalculateFitness();
        generation++;

        List<Genome> champions = new List<Genome>();
        List<Genome> children = new List<Genome>();

        //The champion of each species with more than five networks was copied into the next generation unchanged
        foreach (Species s in species)
            if (s.GetCount() > 5)
                champions.Add(GenomeUtils.Clone(s.GetBestGenome()));

        //In each generation, 25% of offspring resulted from mutation without crossover.
        for (int i = 0; i < GenomeUtils.ASEXUAL_RATE; i++)
        {
            children.Add(GenomeUtils.Clone(nnets[i].genome));
        }

        //Remaining will be crossovers
        for (int i = champions.Count + children.Count; i < Utils.POP_SIZE; i++)
        {
            //pick random species
            Species s = species[Random.Range(0, species.Count)];

            Genome parent1 = s.GetRandomGenome();
            Genome parent2 = s.GetRandomGenome();

            children.Add(GenomeUtils.Crossover(parent1, parent2));
        }

        //Mutate all children

        foreach (Genome g in children)
        {
            if (Random.value < GenomeUtils.MUTATION_RATE)
                g.Mutate();
            if (Random.value < GenomeUtils.ADD_CONNECTION_RATE)
                g.AddConnectionMutation();
            if (Random.value < GenomeUtils.ADD_NODE_RATE)
                g.AddNodeMutation();
        }



        genomes = new List<Genome>(champions);
        genomes = genomes.Concat(children).ToList();

        SetSpecies();
        MakeNNets();
    }
    
}
