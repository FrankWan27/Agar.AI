using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Brain 
{
    Player player;
    GameManager gm;
    public NNet nnet;

    public Brain(Player p, GameManager gm)
    {
        player = p;
        this.gm = gm;
    }

    public List<float> GetInputs()
    {
        List<float> inputs = new List<float>();
        //First input is size
        inputs.Add(player.size);

        //Get 1 closest players xDiff, yDiff, sizeDiff
        List<Blob> toConsider = new List<Blob>();
        foreach(PlayerController pc in gm.players)
        {
            if (pc.player != player)
            {
                toConsider.Add(new Blob(player, pc.player.x, pc.player.y, player.size));
            }
        }
        toConsider.Sort();
        for(int i = 0; i < 3; i++)
        {
            if (toConsider.Count > i)
            {
                inputs.Add(toConsider[i].xDiff);
                inputs.Add(toConsider[i].yDiff);
                inputs.Add(toConsider[i].sizeDiff);
            }
            else
            {
                inputs.Add(0f);
                inputs.Add(0f);
                inputs.Add(0f);
            }
        }

        //Get 1 closest food x, y
        toConsider = new List<Blob>();
        foreach (Food food in gm.foods)
        {
            toConsider.Add(new Blob(player, food.x, food.y, 1));
        }
        toConsider.Sort();
        for (int i = 0; i < 3; i++)
        {
            if (toConsider.Count > i)
            {
                inputs.Add(toConsider[i].xDiff);
                inputs.Add(toConsider[i].yDiff);
            }
            else
            {
                inputs.Add(0f);
                inputs.Add(0f);
            }
        }


        return inputs;
    }

    public void EvaluateFitness()
    {
        nnet.fitness = player.size;
    }

    public float[] GetOutput()
    {
        return nnet.GetOutput(GetInputs().ToArray());
    }

    class Blob : IComparable<Blob>
    {
        protected Player player;
        public float x;
        public float y;
        public float size;
        public float xDiff;
        public float yDiff;
        public float sizeDiff;
        public float distance;

        public int CompareTo(Blob other) //sort in desc order
        {
            return distance.CompareTo(other.distance);
        }

        public Blob(Player player, float x, float y, float size)
        {
            this.player = player;
            this.x = x;
            this.y = y;
            this.size = size;
            xDiff = x - player.x;
            yDiff = y - player.y;
            sizeDiff = size - player.size;
            distance = Mathf.Abs(xDiff) + Mathf.Abs(yDiff);
        }
    }


}
