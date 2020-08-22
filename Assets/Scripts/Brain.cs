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
        //Self Size
        //inputs.Add(player.size / Utils.MAX_SIZE);

        //Get 2 closest players xDiff, yDiff, sizeDiff
        int xClosest = 2;
        List<Blob> toConsider = new List<Blob>();
        foreach(PlayerController pc in gm.players)
        {
            if (pc.player != player)
            {
                toConsider.Add(new Blob(player, pc.player.x, pc.player.y, pc.player.size));
            }
        }
        toConsider.Sort();
        for(int i = 0; i < xClosest; i++)
        {
            if (toConsider.Count > i)
            {
                inputs.Add(toConsider[i].angle);
                inputs.Add(toConsider[i].distance);
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
        Tuple<int, int> coord = Utils.GetCoordinate(player.x, player.y);
        //check closest 15 squares
        int searchRadius = 2;
        for(int xDiff = -searchRadius; xDiff <= searchRadius; xDiff++)
        {
            for(int yDiff = -searchRadius; yDiff <= searchRadius; yDiff++)
            {
                int newX = coord.Item1 + xDiff;
                int newY = coord.Item2 + yDiff;
                if (newX >= -Utils.GAME_WIDTH / 2 && newX <= Utils.GAME_WIDTH / 2)
                {
                    if(newY >= -Utils.GAME_HEIGHT / 2 && newY <= Utils.GAME_HEIGHT / 2)
                    {
                        List<Food> foods;
                        if (gm.foodMap.TryGetValue(new Tuple<int, int>(newX, newY), out foods))
                        {
                            foreach (Food food in foods)
                            {
                                toConsider.Add(new Blob(player, food.x, food.y, 1));
                            }
                        }
                    }
                }
            }
        }
        toConsider.Sort();
        for (int i = 0; i < xClosest; i++)
        {
            if (toConsider.Count > i)
            {
                inputs.Add(toConsider[i].angle);
                inputs.Add(toConsider[i].distance);
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
        gm.SetHighscore(nnet.fitness);
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
        public float angle;

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
            angle = Mathf.Atan2(yDiff, xDiff) / (2 * Mathf.PI);
            distance = Mathf.Sqrt(xDiff * xDiff + yDiff * yDiff) / Utils.MAX_DIST;
        }
    }


}
