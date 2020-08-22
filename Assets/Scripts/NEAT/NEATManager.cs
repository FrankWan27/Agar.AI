using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;

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
    List<NNet> unusedNNets;
    Dictionary<Genome, Species> speciesMap;
    HashSet<Genome> survivors;

    void Start()
    {
        generation = 1;
        population = GenomeUtils.POP_SIZE;

        survivors = new HashSet<Genome>();
        genomes = new List<Genome>();
        unusedNNets = new List<NNet>();
        species = new List<Species>();
        speciesMap = new Dictionary<Genome, Species>();

        for(int i = 0; i < population; i++)
        {
            Genome genome = new Genome(inputNodes, outputNodes);
            genomes.Add(genome);
        }
        SetSpecies();
        MakeNNets();
    }

    void WriteGenome(Genome g, string filename)
    {
        string path = "Resources/" + filename + ".genome";

        StreamWriter writer = new StreamWriter(path, false);

        writer.WriteLine("Nodes");
        foreach (NodeGene node in g.GetNodeGenes().Values)
        {
            writer.WriteLine(node.id + "," + node.type + "," + node.activation);
        }
        writer.WriteLine("Connections");
        foreach (ConnectionGene con in g.GetConnectionGenes().Values)
        {
            writer.WriteLine(con.innovation + "," + con.expressed + "," + con.inNode + "," + con.outNode + "," + con.weight);
        }
        writer.Close();
    }

    void WriteGenome(Genome g)
    {
        WriteGenome(g, "fitness " + (int)g.nnet.fitness);
    }

    public void LoadGenome(string filename, bool all)
    {
        string path = filename;

        Genome genome = new Genome();
        using (StreamReader reader = new StreamReader(path))
        {
            bool node = false;
            bool connection = false;
            string line;
            while((line = reader.ReadLine()) != null)
            {
                if (line.Equals("Nodes"))
                {
                    node = true;
                    connection = false;
                }
                else if (line.Equals("Connections"))
                {
                    node = false;
                    connection = true;
                }
                else
                {
                    string[] info = line.Split(',');

                    if (node)
                    {
                        int id = int.Parse(info[0]);
                        NodeGene.Type type = (NodeGene.Type)System.Enum.Parse(typeof(NodeGene.Type), info[1]);
                        GenomeUtils.Activation activation = (GenomeUtils.Activation)System.Enum.Parse(typeof(GenomeUtils.Activation), info[2]);
                        genome.AddNodeGene(new NodeGene(type, id, activation));
                    }
                    else if(connection)
                    {
                        int innovation = int.Parse(info[0]);
                        bool expressed = bool.Parse(info[1]);
                        int inNode = int.Parse(info[2]);
                        int outNode = int.Parse(info[3]);
                        float weight = float.Parse(info[4]);
                        genome.AddConnectionGene(new ConnectionGene(inNode, outNode, weight, expressed, innovation));
                    }
                    else
                        Debug.LogError("Invalid genome file");
                }
            }
        }

        genomes.Clear();
        genomes.Add(genome);

        if (all)
            for(int i = 1; i < GenomeUtils.POP_SIZE; i++)
                genomes.Add(GenomeUtils.Clone(genome));
        else
            for (int i = 1; i < GenomeUtils.POP_SIZE; i++)
                genomes.Add(new Genome(inputNodes, outputNodes));

        SetSpecies();
        MakeNNets();
    }

    void MakeNNets()
    {
        unusedNNets.Clear();
        foreach(Genome g in genomes)
        {
            NNet nnet = new NNet(g);
            unusedNNets.Add(nnet);
        }
    }

    public (NNet, bool) GetNNet()
    {
        if(unusedNNets.Count <= 0)
        {
            Debug.LogError("No NNets Remaining");
            return (null, false);
        }
        NNet unusedNNet = unusedNNets[0];
        bool isChampion = survivors.Contains(unusedNNet.genome);
        unusedNNets.RemoveAt(0);
        return (unusedNNet, isChampion);
    }

    void SetSpecies()
    {
        species.Clear();
        speciesMap.Clear();

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
        foreach(Genome g in genomes)
        {
            //g.nnet.fitness = g.nnet.fitness / speciesMap[g].GetCount();
            speciesMap[g].fitness += g.nnet.fitness;
        }

        genomes.Sort();
        species.Sort();

        WriteGenome(genomes[0], "generation " + generation + " fitness " + (int)genomes[0].nnet.fitness);
    }
    public void NextGeneration()
    {
        CalculateFitness();
        generation++;

        survivors.Clear();
        List<Genome> children = new List<Genome>();

        int numChampions = (int)(GenomeUtils.POP_SIZE * GenomeUtils.CHAMPION_RATE);
        int numASexual = (int)(GenomeUtils.POP_SIZE * GenomeUtils.ASEXUAL_RATE);

        //The top 10% of nnets will be reproduced untouched
        for (int i = 0; i < numChampions; i++)
        {
            Genome champ = GenomeUtils.Clone(genomes[i]);
            children.Add(champ);
            survivors.Add(champ);
        }

        //In each generation, 25% of offspring resulted from mutation without crossover.
        for (int i = 0; i < numASexual; i++)
        {
            children.Add(GenomeUtils.Clone(genomes[i]));
        }

        //Remaining will be crossovers
        for (int i = children.Count; i < GenomeUtils.POP_SIZE; i++)
        {
            //pick random species
            Species s = species[Random.Range(0, species.Count)];

            Genome parent1 = s.GetRandomGenome();
            Genome parent2 = s.GetRandomGenome();

            children.Add(GenomeUtils.Crossover(parent1, parent2));
        }

        //Mutate all children
        for (int i = (int) (GenomeUtils.POP_SIZE * GenomeUtils.CHAMPION_RATE); i < children.Count; i++)
        {
            if (Random.value < GenomeUtils.MUTATION_RATE)
                children[i].Mutate();
            if (Random.value < GenomeUtils.ADD_CONNECTION_RATE)
                children[i].AddConnectionMutation();
            if (Random.value < GenomeUtils.ADD_NODE_RATE)
                children[i].AddNodeMutation();
        }

        genomes = children;
        

        SetSpecies();
        MakeNNets();
    }

}
